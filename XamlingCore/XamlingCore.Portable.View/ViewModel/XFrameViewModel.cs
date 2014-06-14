using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Autofac;
using XamlingCore.Portable.Contract.Localisation;
using XamlingCore.Portable.Contract.Services;
using XamlingCore.Portable.Messages.Navigation;
using XamlingCore.Portable.Messages.System;
using XamlingCore.Portable.View.Navigation;
using XamlingCore.Portable.View.ViewModel.Base;
using XamlingCore.Portable.XamlingMessenger;

namespace XamlingCore.Portable.View.ViewModel
{
    public class XFrameViewModelBase : ViewModelBase
    {
        private readonly ILoadStatusService _systemTrayService;
        private readonly IApplicationBarService _applicationBarService;
        private readonly IOrientationService _orientationService;
        private readonly ILocalisationService _localisationService;

        private XNavigationService navigationService;

        private object overlayViewModel;

        public ICommand NavigateBackCommand { get; set; }

        private bool _isLoading;
        private bool _isFullScreenLoading;

        public string Title { get; set; }
        public string Description { get; set; }

        public static XFrameViewModelBase RootFrame { get; protected internal set; }

        public Action BackKeyIntercept { get; set; }

        public object BackExitObject { get; set; }

        protected internal List<Type> KnownTypes = new List<Type>();

        public List<object> AllViewModels = new List<object>();
        public List<object> IgnoreResetModels = new List<object>();

        public ILifetimeScope Container { get; private set; }

        public XFrameViewModelBase(
            ILifetimeScope c,
            ILoadStatusService systemTrayService,
            IApplicationBarService applicationBarService,
            IOrientationService orientationService,
            ILocalisationService localisationService)
        {
            _systemTrayService = systemTrayService;
            _applicationBarService = applicationBarService;
            _orientationService = orientationService;
            _localisationService = localisationService;
            NavigationService = new XNavigationService(this);

            Container = c;
        }

        public void ResetNavigationAndHistory()
        {
            var allvmCopy = AllViewModels.ToList();

            foreach (var item in allvmCopy)
            {
                if (IgnoreResetModels.Contains(item))
                {
                    continue;
                }

                if (item == null)
                {
                    return;
                }

                var disp = item as IDisposable;

                if (disp != null)
                {
                    disp.Dispose();
                }
            }
            allvmCopy.Clear();
            AllViewModels.Clear();
            NavigationService.ResetHistory();
            NavigationService.CurrentContentObject = null;
        }

        public static T CreateRootFrame<T>(ILifetimeScope c, Action<T> initialisedCallback = null)
            where T : XFrameViewModelBase
        {
            var obj = c.Resolve<T>();
            obj.KnownTypes.Add(typeof(T));
            if (initialisedCallback != null) initialisedCallback(obj);
            obj.OnInitialise();
            RootFrame = obj;
            return obj;
        }

        public T CreateContentModel<T>()
             where T : XViewModel
        {
            return CreateContentModel<T>(null);
        }

        public T CreateContentModel<T>(Action<T> initialisedCallback)
             where T : XViewModel
        {
            var rootModel = this;

            rootModel.KnownTypes.Add(typeof(T));

            var obj = Container.Resolve<T>();

            if (Dispatcher == null)
            {
                throw new Exception("Dispatcher is null, do soemthing like this in RootViewModel ctor Dispatcher = (a) => Deployment.Current.Dispatcher.BeginInvoke(a);");
            }

            obj.Dispatcher = Dispatcher;

            obj.ParentModel = this;

            obj.SystemTrayService = _systemTrayService;
            obj.ApplicationBarService = _applicationBarService;
            obj.OrientationService = _orientationService;
            obj.LocalisationService = _localisationService;

            if (initialisedCallback != null) initialisedCallback(obj);

            rootModel.AllViewModels.Add(obj);

            obj.OnInitialise();


            return obj;
        }

        public virtual void OnInitialise() { }


        private bool _activatedFired = false;

        /// <summary>
        /// Called when the app comes out of background to foreground
        /// </summary>
        public virtual void OnActivated()
        {
            FireActivated();
        }

        /// <summary>
        /// Called when the app is about to be shoved in to the background
        /// </summary>
        public virtual void OnDeactivated()
        {
            _activatedFired = false;
            var cco = NavigationService.CurrentContentObject as XViewModel;

            if (cco != null)
            {
                cco.OnDeactivated();
            }

            XMessenger.Default.Send(new ApplicationDeactivatedMessage());
        }

        protected void FireActivated()
        {
            if (_activatedFired)
            {
                return;
            }

            _activatedFired = true;

            BackExitObject = null;

            var cco = NavigationService.CurrentContentObject as XViewModel;

            if (cco != null)
            {
                cco.OnActivated();
            }

            XMessenger.Default.Send(new ApplicationReactivatedMessage());
        }

        void navigationService_Navigated(object sender, System.EventArgs e)
        {
            OnPropertyChanged("BackEnabled");
            OnPropertyChanged("BackVisible");

            OnNavigated();
        }

        protected virtual void OnNavigated()
        {

        }

        public virtual void OnNavigating(object newObject, object oldObject)
        {
            _applicationBarService.Reset();
        }

        public bool BackEnabled
        {
            get { return NavigationService.CanGoBack; }
        }

        public bool IsLoading
        {
            get
            {
                return _isLoading;
            }
            set
            {

                _isLoading = value;
                OnPropertyChanged("IsLoading");

                if (value)
                {
                    _systemTrayService.PushLoader();
                }
                else
                {
                    _systemTrayService.PopLoader();
                }
            }
        }

        public bool IsFullScreenLoading
        {
            get
            {
                return _isFullScreenLoading;
            }
            set
            {
                _isFullScreenLoading = value;
                OnPropertyChanged("IsFullScreenLoading");
            }
        }


        public XNavigationService NavigationService
        {
            get { return navigationService; }
            set
            {
                if (navigationService != null)
                {
                    navigationService.Navigated -= new System.EventHandler(navigationService_Navigated);
                }

                navigationService = value;
                OnPropertyChanged("NavigationService");
                navigationService.Navigated += new System.EventHandler(navigationService_Navigated);
            }
        }

        public object CurrentContentObject
        {
            get { return NavigationService.CurrentContentObject; }
        }

        public bool IsCurrentContentObjectType(Type type)
        {
            if (NavigationService.CurrentContentObject == null)
            {
                return false;
            }

            return NavigationService.CurrentContentObject.GetType() == type;
        }

        #region NavigationShortcuts

        public virtual bool OnBackNavigate()
        {
            if (BackExitObject != null && NavigationService.CurrentContentObject == BackExitObject)
            {
                Debug.WriteLine("WARNING: BackExitObject is quitting the application");
                new ForceAppExitUsingNavigation().Send();
                return false;

            }

            if (NavigationService.CanGoBack || BackKeyIntercept != null)
            {
                NavigateBack();
                return true;
            }

            Debug.WriteLine("WARNING: Back press is quitting the application");
            return false;
        }

        public void NavigateSkipBack<T>()
            where T : XViewModel
        {
            if (NavigationService.CurrentContentObject != null && NavigationService.CurrentContentObject.GetType() == typeof(T))
            {
                return;
            }

            var nh = NavigationService.NavigationHistory;

            var item = nh.FirstOrDefault(_ => _.GetType() == typeof(T));

            if (item == null)
            {
                NavigateTo<T>();
                return;
            }

            var index = nh.IndexOf(item);

            for (var i = index + 1; i < nh.Count; i++)
            {
                var obj = nh[i] as XViewModel;
                if (obj != null)
                {
                    obj.Dispose();
                }
            }

            NavigateTo(item);
        }

        public void NavigateHome()
        {
            if (NavigationService.NavigationHistory.Count > 0)
            {
                if (NavigationService.NavigationHistory[0] != NavigationService.CurrentContentObject)
                {
                    NavigateTo(NavigationService.NavigationHistory[0]);
                }
            }

        }

        public void NavigateTo<T>(Action<T> initialisedCallback = null, bool noHistory = false)
           where T : XViewModel
        {
            var vm = CreateContentModel<T>(initialisedCallback);
            NavigateTo(vm, noHistory);

        }

        public void NavigateTo(object content)
        {
            NavigationService.NavigateTo(content, false);
        }


        public void NavigateTo(object content, bool noHistory)
        {
            NavigationService.NavigateTo(content, noHistory, false);
        }

        public void NavigateTo(object content, bool noHistory, bool forceBack)
        {
            NavigationService.NavigateTo(content, noHistory, forceBack);
        }

        public void NavigateBack()
        {
            if (BackKeyIntercept != null)
            {
                BackKeyIntercept();
            }
            else
            {
                NavigationService.NavigateBack(false);
            }
        }

        public void NavigateBack(bool allowNullNavigation)
        {
            NavigationService.NavigateBack(allowNullNavigation);
        }

        public bool IsInNavigationCooldown()
        {
            return NavigationService.IsInNavigationCooldown();
        }

        #endregion
    }
}
