using System;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Autofac;
using Xamarin.Forms;
using XamlingCore.Portable.Data.Glue;
using XamlingCore.XamarinThings.Contract;

namespace XamlingCore.XamarinThings.Content.Dynamic
{
    public class DynamicContentTransitionView : ContentView, IDisposable
    {
        private BindableObject _bindingParent;

        private bool _isDisposed;

        private DynamicContentHostView _hostView;

        private bool _currentIsOne = true;

        public DynamicContentTransitionView(BindableObject bindingParent)
        {
            _bindingParent = bindingParent;
            IsVisible = false;
            SetBindingParent(bindingParent);
        }
        public async void SetBindingParent(BindableObject v)
        {
            if (v == null)
            {
                return;
            }

            while (v.BindingContext == null)
            {
                await Task.Yield();
            }

            if (BindingContext == null)
            {
                BindingContext = v.BindingContext;
            }

            v.BindingContextChanged += v_BindingContextChanged;
        }

        public DynamicContentTransitionView()
        {

        }

        protected override void OnParentSet()
        {
            base.OnParentSet();

            if (Parent == null)
            {
                Dispose();
                return;
            }

            if (BindingContext == null)
            {
                SetBindingParent(Parent);
            }
        }

        void v_BindingContextChanged(object sender, EventArgs e)
        {
            var bo = sender as BindableObject;

            if (bo == null)
            {
                return;
            }

            BindingContext = bo.BindingContext;

        }

        public object DataContext
        {
            get { return GetValue(DataContextProperty); }
            set
            {
                SetValue(DataContextProperty, value);
            }
        }


        public static readonly BindableProperty DataContextProperty =
            BindableProperty.Create<DynamicContentView, object>(p => p.DataContext, null, BindingMode.OneWay, null, _onDataContextChanged);


        private async static void _onDataContextChanged(BindableObject obj, object oldValue, object newValue)
        {
            if (obj.BindingContext == null)
            {
                return;
            }

            var viewer = obj as DynamicContentTransitionView;
            
            if (viewer == null)
            {
                return;
            }

            if (viewer._isDisposed)
            {
                return;
            }

            viewer._createContentHost();

            if (newValue == null)
            {
                if (viewer._isDisposed)
                {
                    return;
                }
                viewer._transitionOut();
                return;
            }

            var vm = newValue;

            //resolve a view for this bad boy
            var resolver = ContainerHost.Container.Resolve<IViewResolver>();

            var v = resolver.ResolveView(vm);

            if (v == null)
            {
                throw new InvalidOperationException("Could not find view for this view model: " + vm.GetType().FullName);
            }

            if (viewer._isDisposed)
            {
                return;
            }

            try
            {
                viewer._setNewView(v);
            }
            catch
            {

            }
        }

        void _createContentHost()
        {
            if (_hostView == null)
            {
                _hostView = new DynamicContentHostView();
                Content = _hostView;
            }
        }

        async void _setNewView(View view)
        {
            _currentIsOne = !_currentIsOne;

            var n = _getNewView();

            n.Content = view;
            
            _hostView.RaiseChild(n);

            await Task.Yield();

            n.IsVisible = true;

            _transitionOut();

            _transitionIn();
        }

        void _transitionOut()
        {
            try
            {
                Task.Run(async () =>
                {
                    await Task.Yield();
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        await Task.Yield();
                        var v = _getOldView();

                        var tv = v.Content as ITransitionView;

                        if (tv != null)
                        {
                            await tv.TransitionOut();
                        }

                        v.IsVisible = false;
                        v.Content = null;
                    });
                });
                
            }
            catch { }
        }

        

        void _transitionIn()
        {
            try
            {
                Task.Run(async () =>
                {
                    await Task.Yield();
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        await Task.Yield();

                        var v = _getNewView();

                        var tv = v.Content as ITransitionView;

                        if (tv != null)
                        {
                            tv.TransitionIn();
                        }
                    });
                });

            }
            catch { }
        }

        ContentView _getOldView()
        {
            return _currentIsOne ? _hostView.ContentTwo : _hostView.ContentOne;
        }

        ContentView _getNewView()
        {
            return !_currentIsOne ? _hostView.ContentTwo : _hostView.ContentOne;
        }


        public virtual void Dispose()
        {
            if (_bindingParent != null)
            {
                _bindingParent.BindingContextChanged -= v_BindingContextChanged;
                _bindingParent = null;
            }
            BindingContext = null;
            DataContext = null;
        }
    }
}
