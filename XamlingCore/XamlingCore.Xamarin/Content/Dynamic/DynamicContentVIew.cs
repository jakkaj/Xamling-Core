using System;
using System.Threading.Tasks;
using Autofac;
using Xamarin.Forms;
using XamlingCore.Portable.Data.Glue;
using XamlingCore.XamarinThings.Contract;

namespace XamlingCore.XamarinThings.Content.Dynamic
{
    public class DynamicContentView : ContentView
    {
        public DynamicContentView(BindableObject bindingParent)
        {
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
            var viewer = obj as DynamicContentView;
            if (viewer == null)
            {
                return;
            }

            if (newValue == null)
            {
                if (!await viewer.ContentSetOverride(null))
                {
                    viewer.Content = null;
                    viewer.IsVisible = false;
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
                viewer.Content = v;
                viewer.IsVisible = true;
            }
            
            
        }

        protected virtual async Task<bool> ContentSetOverride(View content)
        {
            return false;
        }
    }
}
