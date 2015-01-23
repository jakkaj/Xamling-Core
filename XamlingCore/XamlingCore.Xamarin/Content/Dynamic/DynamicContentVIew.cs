using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Autofac;
using Xamarin.Forms;
using XamlingCore.Portable.Data.Glue;
using XamlingCore.XamarinThings.Contract;

namespace XamlingCore.XamarinThings.Content.Dynamic
{
    public class DynamicContentView : ContentView, IDisposable
    {
        private BindableObject _bindingParent;

        private bool _isDisposed;

        public DynamicContentView(BindableObject bindingParent)
        {
            _bindingParent = bindingParent;
            IsVisible = false;
            SetBindingParent(bindingParent);
        }
        public void SetBindingParent(BindableObject v)
        {
            if (v == null)
            {
                return;
            }

            BindingContext = v.BindingContext;

            v.BindingContextChanged += v_BindingContextChanged;
        }

        public DynamicContentView()
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
            BindableProperty.Create<DynamicContentView, object>(p => p.DataContext, null, BindingMode.TwoWay, null, _onDataContextChanged);


        private async static void _onDataContextChanged(BindableObject obj, object oldValue, object newValue)
        {
            if (obj.BindingContext == null)
            {
                return;
            }

            var viewer = obj as DynamicContentView;
            if (viewer == null)
            {
                return;
            }

            if (viewer._isDisposed)
            {
                return;
            }

            if (newValue == null)
            {
                if (!await viewer.ContentSetOverride(null))
                {
                    if (viewer._isDisposed)
                    {
                        return;
                    }
                    try
                    {
                        viewer.IsVisible = false;
                        viewer.Content = null;
                    }
                    catch { }

                }
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

            if (!await viewer.ContentSetOverride(v))
            {
                if (viewer._isDisposed)
                {
                    return;
                }

                try
                {
                    viewer.IsVisible = true;
                    viewer.Content = v;
                }
                catch
                {
                    
                }
                

            }
        }

        protected virtual async Task<bool> ContentSetOverride(View content)
        {
            return false;
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
