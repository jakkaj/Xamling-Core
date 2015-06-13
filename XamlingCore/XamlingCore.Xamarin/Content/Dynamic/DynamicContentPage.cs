using System;
using System.Threading.Tasks;
using Autofac;
using Xamarin.Forms;
using XamlingCore.Portable.Data.Glue;
using XamlingCore.XamarinThings.Contract;

namespace XamlingCore.XamarinThings.Content.Dynamic
{
    public class DynamicContentPage : ContentPage
    {
        protected override async void OnBindingContextChanged()
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

            Content = v;

            SetBinding(TitleProperty, new Binding("Title"));

            var icon = Content as IIconView;
            if (icon != null)
            {
                Icon = icon.Icon;
            }

            base.OnBindingContextChanged();
        }
    }
}
