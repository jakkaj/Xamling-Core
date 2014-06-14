using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using XamlingCore.Portable.Contract.Localisation;
using XamlingCore.Portable.Contract.Services;
using XamlingCore.Portable.Messages.Device;
using XamlingCore.Portable.Model.Orientation;
using XamlingCore.Portable.View.ViewModel.Base;
using XamlingCore.Portable.XamlingMessenger;

namespace XamlingCore.Portable.View.ViewModel
{
    public abstract class XViewModel : ViewModelBase, IDisposable
    {
        private XRootViewModelBase _parentModel;

        public bool IsDisposed { get; private set; }
        public double ScrollPosition { get; set; }
        public object Tag { get; set; }

        public event EventHandler Disposed;
        public event EventHandler Activated;

        private string _title;
        private string _description;

        private bool _isLandscape;

        private bool _isThisLoading;

        private readonly List<XViewModel> _childModels = new List<XViewModel>();

        protected internal ILoadStatusService LoadStatusService;
        protected internal IOrientationService OrientationService;
        protected internal ILocalisationService LocalisationService;

        public T CreateContentModel<T>(Action<T> initialisedCallback)
             where T : XViewModel
        {
            var model = ParentModel.CreateContentModel<T>(initialisedCallback);

            _childModels.Add(model);

            return model;
        }

        public T CreateContentModel<T>()
             where T : XViewModel
        {
            var model = ParentModel.CreateContentModel<T>(null);

            _childModels.Add(model);

            return model;
        }

        public virtual void OnInitialise()
        {
            XMessenger.Default.Register<PageOrientationChangedMessage>(this, WrapCall(_setOrientation));
            _setOrientation();
        }
        public virtual void OnDeactivated() { }

        public virtual void OnActivated()
        {
            if (Activated != null)
            {
                Activated(this, EventArgs.Empty);
            }
        }

        public bool IsActive
        {
            get
            {
                if (ParentModel.Navigation.CurrentContentObject == this)
                {
                    return true;
                }

                var vm = ParentModel.Navigation.CurrentContentObject as XViewModel;
                
                if (vm == null)
                {
                    return false;
                }

                if (vm._childModels.Contains(this))
                {
                    return true;
                }

                return false;
            }
        }

        public virtual void Dispose()
        {
            if (IsDisposed) return;

            XMessenger.Default.Unregister(this);

            _parentModel.AllViewModels.Remove(this);

            foreach (var child in _childModels)
            {
                if (!child.IsDisposed && child != ParentModel.Navigation.CurrentContentObject)
                {
                    child.Dispose();
                }
            }

            _childModels.Clear();

            PropertyDispose();

            Debug.WriteLine("***Dispose*** " + _parentModel.AllViewModels.Count);

            if (Disposed != null)
            {
                Disposed(this, EventArgs.Empty);
                Disposed = null;
            }

            IsDisposed = true;
        }

        protected Action<object> WrapCall(Action<object> method)
        {
            return (obj) => Dispatcher(() => method(obj));
        }

        protected Action<object> WrapCallTyped<T>(Action<T> method)
            where T : class
        {
            return (obj) => Dispatcher(() => method(obj as T));
        }

        protected async Task<T> Loader<T>(Task<T> awaiter, string text = null)
        {
            return await LoadStatusService.Loader(awaiter, text);
        }

        protected async Task Loader(Task awaiter, string text = null)
        {
            await LoadStatusService.Loader(awaiter, text);
        }

        protected string GetResource(string resourceName)
        {
            return LocalisationService.GetLocalisedResource(resourceName);
        }

        void _setOrientation(object obj = null)
        {
            IsLandscape = OrientationService.CurrentPageOrientation == XPageOrientation.Landscape;
        }

        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                OnPropertyChanged("Title");
            }
        }


        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                OnPropertyChanged("Description");
            }
        }

        /// <summary>
        /// Shows system wide loading status, say in the system tray.
        /// </summary>
        public bool IsLoading
        {
            get
            {
                return ParentModel.IsLoading;
            }
            set
            {
                ParentModel.IsLoading = value;
            }
        }

        /// <summary>
        /// Use to loading status on the current item, like an in place loader (which you will have to add). 
        /// </summary>

        public bool IsThisLoading
        {
            get { return _isThisLoading; }
            set
            {
                _isThisLoading = value;
                OnPropertyChanged();
            }
        }

        public bool IsFullScreenLoading
        {
            get
            {
                return ParentModel.IsFullScreenLoading;
            }
            set
            {
                ParentModel.IsFullScreenLoading = value;
            }
        }

        protected internal XRootViewModelBase ParentModel
        {
            get
            {
                return _parentModel;
            }
            set
            {
                _parentModel = value;
            }
        }

        public bool IsLandscape
        {
            get { return _isLandscape; }
            set
            {
                _isLandscape = value;
                OnPropertyChanged();
            }
        }

        #region NavigationShortcuts

        public void NavigateSkipBack<T>()
            where T : XViewModel
        {
            ParentModel.NavigateSkipBack<T>();
        }

        public void NavigateTo<T>(Action<T> initialisedCallback = null, bool noHistory = false)
            where T : XViewModel
        {
            var vm = CreateContentModel<T>(initialisedCallback);
            NavigateTo(vm, noHistory);

        }

        public void NavigateTo(object content)
        {
            ParentModel.NavigateTo(content);
        }

        protected void NavigateTo(object content, bool noHistory, bool forceBack)
        {
            ParentModel.NavigateTo(content, noHistory, forceBack);
        }

        protected void NavigateTo(object content, bool noHistory)
        {
            ParentModel.NavigateTo(content, noHistory);
        }

        protected void NavigateBack()
        {
            ParentModel.NavigateBack();
        }

        protected void NavigateBack(bool allowNullNavigation)
        {
            ParentModel.NavigateBack(allowNullNavigation);
        }

        public bool IsInNavigationCooldown()
        {
            return ParentModel.IsInNavigationCooldown();
        }
        #endregion
    }
}
