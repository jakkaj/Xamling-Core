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
        protected override void OnAppearing()
        {
            base.OnAppearing();
            _doContent();
        }

        async void _doContent()
        {
            await Task.Yield();

            var vm = BindingContext;

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
