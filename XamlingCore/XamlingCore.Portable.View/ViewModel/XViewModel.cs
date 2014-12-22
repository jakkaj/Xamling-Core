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
using XamlingCore.Portable.Messages.Device;
using XamlingCore.Portable.Messages.XamlingMessenger;
using XamlingCore.Portable.Model.Orientation;
using XamlingCore.Portable.View.ViewModel.Base;

namespace XamlingCore.Portable.View.ViewModel
{
    public abstract class XViewModel : ViewModelBase, IDisposable
    {
        private XFrame _parentModel;

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

        public ILifetimeScope Container { get; internal set; }

        private XViewModel _ancillaryViewModel;

        public static Func<string, string, string, string, Task<bool>> NativeAlert { get; set; }

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

            //Debug.WriteLine("***Dispose*** " + _parentModel.AllViewModels.Count);

            if (Disposed != null)
            {
                Disposed(this, EventArgs.Empty);
                Disposed = null;
            }

            IsDisposed = true;
        }

        protected async Task<bool> DisplayAlert(string title, string message, string accept, string cancel)
        {
            if (NativeAlert == null)
            {
                return false;
            }

            var t = GetResource(title) ?? title;
            var m = GetResource(message) ?? message;
            var a = GetResource(accept) ?? accept;
            var c = GetResource(cancel) ?? cancel;

            return await NativeAlert(t, m, a, c);
        }

        protected Action<object> WrapCall(Action<object> method)
        {
            return (obj) => Dispatcher.Invoke(() => method(obj));
        }

        protected Action<object> WrapCallTyped<T>(Action<T> method)
            where T : class
        {
            return (obj) => Dispatcher.Invoke(() => method(obj as T));
        }

        public async Task<T> Loader<T>(Task<T> awaiter, string text = null)
        {
            return await LoadStatusService.Loader(awaiter, text);
        }

        public async Task Loader(Task awaiter, string text = null)
        {
            await LoadStatusService.Loader(awaiter, text);
        }

        public string GetResource(string resourceName)
        {
            if (resourceName == null)
            {
                return null;
            }
            return LocalisationService.Get(resourceName);
        }

        void _setOrientation(object obj = null)
        {
            IsLandscape = OrientationService.CurrentPageOrientation == XPageOrientation.Landscape;
        }

        void _onParentModelSet()
        {
            if (_childModels == null)
            {
                return;
            }

            foreach (var c in _childModels)
            {
                c.ParentModel = ParentModel;
            }
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
    
        public XFrame ParentModel
        {
            get
            {
                return _parentModel;
            }
            set
            {
                _parentModel = value;
                _onParentModelSet();
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

        public XViewModel AncillaryViewModel
        {
            get { return _ancillaryViewModel; }
            set
            {
                _ancillaryViewModel = value;
                OnPropertyChanged();
            }
        }

        #region NavigationShortcuts

        public void NavigateSkipBack<T>()
            where T : XViewModel
        {
            ParentModel.NavigateSkipBack<T>();
        }

        public void NavigateToModal(object content)
        {
            ParentModel.NavigateToModal(content);
        }

        public void NavigateToModal<T>(Action<T> initialisedCallback = null, bool noHistory = false)
            where T : XViewModel
        {
            var vm = CreateContentModel<T>(initialisedCallback);
            NavigateToModal(vm);
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

        public void NavigateBack()
        {
            ParentModel.NavigateBack();
        }

        public void NavigateBack(bool allowNullNavigation)
        {
            ParentModel.NavigateBack(allowNullNavigation);
        }
        
        #endregion
    }
}
