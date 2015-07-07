using System;
using System.Threading.Tasks;
using Autofac;
using Xamarin.Forms;
using XamlingCore.Portable.Data.Glue;
using XamlingCore.XamarinThings.Contract;

namespace XamlingCore.XamarinThings.Content.Dynamic
{
    public class DynamicContentCell : ViewCell
    {
        private object _previousBindingContext;

        protected override void OnBindingContextChanged()
        {
            _doContent();
            base.OnBindingContextChanged();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            if (Device.OS == TargetPlatform.Windows)
            {
                await Task.Yield();
            }

            _doContent();
        }

        void _doContent()
        {
            var vm = BindingContext;

            if (_previousBindingContext == vm)
            {
                return;
            }

            _previousBindingContext = vm;

            //resolve a view for this bad boy
            var resolver = ContainerHost.Container.Resolve<IViewResolver>();

            var v = resolver.ResolveView(vm);

            if (v == null)
            {
                throw new InvalidOperationException("Could not find view for this view model: " + vm.GetType().FullName);
            }

            View = v;

        }
    }

    
}
