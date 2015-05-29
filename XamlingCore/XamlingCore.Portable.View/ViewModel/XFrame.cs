using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Autofac;
using XamlingCore.Portable.Contract.Localisation;
using XamlingCore.Portable.Contract.Navigation;
using XamlingCore.Portable.Contract.Services;
using XamlingCore.Portable.Contract.UI;
using XamlingCore.Portable.Contract.ViewModels;
using XamlingCore.Portable.Messages.Navigation;
using XamlingCore.Portable.Messages.System;
using XamlingCore.Portable.Messages.XamlingMessenger;
using XamlingCore.Portable.Model.Navigation;
using XamlingCore.Portable.View.Navigation;
using XamlingCore.Portable.View.ViewModel.Base;

namespace XamlingCore.Portable.View.ViewModel
{
    public abstract class XFrame : ViewModelBase
    {
        private bool _isReady;

        public Guid Id { get; private set; }

        private readonly ILoadStatusService _loadStatusService;
        private readonly IOrientationService _orientationService;
        private readonly ILocalisationService _localisationService;

        private IXNavigation _navigation;

        private bool _isLoading;
        private bool _isFullScreenLoading;
        private bool _isModal;

        public string Title { get; set; }
        public string Description { get; set; }

        public static XFrame RootRoot { get; protected internal set; }

        public Action BackKeyIntercept { get; set; }

        public object BackExitObject { get; set; }

        protected internal List<Type> KnownTypes = new List<Type>();

        public List<object> AllViewModels = new List<object>();
        public List<object> IgnoreResetModels = new List<object>();

        public ILifetimeScope Container { get; private set; }

        protected XFrame(
            ILifetimeScope c,
            ILoadStatusService systemTrayService,
            IOrientationService orientationService,
            ILocalisationService localisationService, 
            IXNavigation xNavigationService, 
            IDispatcher dispatcher)
        {
            _loadStatusService = systemTrayService;
            _orientationService = orientationService;
            _localisationService = localisationService;
            _navigation = xNavigationService;
            Dispatcher = dispatcher;
            Container = c;

            Id = Guid.NewGuid();
        }

        public abstract void Init();

        public void SetReady()
        {
            IsReady = true;
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
            Navigation.ResetHistory();
            Navigation.CurrentContentObject = null;
        }

        public static T CreateRootFrame<T>(ILifetimeScope c, Action<T> initialisedCallback = null)
            where T : XFrame
        {
            var obj = c.Resolve<T>();
            obj.KnownTypes.Add(typeof(T));
            if (initialisedCallback != null) initialisedCallback(obj);
            obj.OnInitialise();
            RootRoot = obj;
            return obj;
        }

        public T CreateContentModel<T>(XViewModel createdBy = null)
             where T : XViewModel
        {
            return CreateContentModel<T>(null, createdBy);
        }

        public T CreateContentModel<T>(Action<T> initialisedCallback, XViewModel createdBy = null)
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
            obj.ParentViewModel = createdBy;

            if (createdBy != null)
            {
                obj.Tag = createdBy.Tag;
            }

            obj.LoadStatusService = _loadStatusService;
            
            obj.OrientationService = _orientationService;
            obj.LocalisationService = _localisationService;

            obj.Container = Container;

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
            var cco = Navigation.CurrentContentObject as XViewModel;

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

            var cco = Navigation.CurrentContentObject as XViewModel;

            if (cco != null)
            {
                cco.OnActivated();
            }

            XMessenger.Default.Send(new ApplicationReactivatedMessage());
        }

        void navigationService_Navigated(object sender, XNavigationEventArgs e)
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
            
        }

        public bool BackEnabled
        {
            get { return Navigation.CanGoBack; }
        }


        public IXNavigation Navigation
        {
            get { return _navigation; }
            set
            {
                if (_navigation != null)
                {
                    _navigation.Navigated -= navigationService_Navigated;
                }
                
                _navigation = value;
                OnPropertyChanged("Navigation");
                _navigation.Navigated += navigationService_Navigated;
            }
        }

        public object CurrentContentObject
        {
            get { return Navigation.CurrentContentObject; }
        }

        public bool IsReady
        {
            get { return _isReady; }
            set
            {
                _isReady = value;
                OnPropertyChanged();
            }
        }

        public bool IsCurrentContentObjectType(Type type)
        {
            if (Navigation.CurrentContentObject == null)
            {
                return false;
            }

            return Navigation.CurrentContentObject.GetType() == type;
        }

        public bool IsModal
        {
            get { return _isModal; }
            set
            {
                _isModal = value;
                _navigation.IsModal = value;
                OnPropertyChanged();
            }
        }

        #region NavigationShortcuts

        public virtual bool OnBackNavigate()
        {
            if (BackExitObject != null && Navigation.CurrentContentObject == BackExitObject)
            {
                Debug.WriteLine("WARNING: BackExitObject is quitting the application");
                new ForceAppExitUsingNavigation().Send();
                return false;

            }

            if (Navigation.CanGoBack || BackKeyIntercept != null)
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
            if (Navigation.CurrentContentObject != null && Navigation.CurrentContentObject.GetType() == typeof(T))
            {
                return;
            }

            var nh = Navigation.NavigationHistory;

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
            if (Navigation.NavigationHistory.Count > 0)
            {
                if (Navigation.NavigationHistory[0] != Navigation.CurrentContentObject)
                {
                    NavigateTo(Navigation.NavigationHistory[0]);
                }
            }

        }

        public void NavigateTo<T>(Action<T> initialisedCallback = null, bool noHistory = false)
           where T : XViewModel
        {
            var vm = CreateContentModel<T>(initialisedCallback);
            NavigateTo(vm, noHistory);

        }

        public void NavigateToModal(object content)
        {
            Navigation.NavigateToModal(content);
        }

        public void NavigateTo(object content)
        {
            Navigation.NavigateTo(content, false);
        }


        public void NavigateTo(object content, bool noHistory)
        {
            Navigation.NavigateTo(content, noHistory, false);
        }

        public void NavigateTo(object content, bool noHistory, bool forceBack)
        {
            Navigation.NavigateTo(content, noHistory, forceBack);
        }

        public void NavigateBack()
        {
            if (BackKeyIntercept != null)
            {
                BackKeyIntercept();
            }
            else
            {
                Navigation.NavigateBack(false);
            }
        }

        public void NavigateBack(bool allowNullNavigation)
        {
            Navigation.NavigateBack(allowNullNavigation);
        }

        #endregion
    }
}
