using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Xamarin.Forms;
using XamlingCore.XamarinThings.Container;
using XamlingCore.XamarinThings.Contract;

namespace XamlingCore.XamarinThings.Content.Cells
{
    public class DynamicContentPage : ContentPage
    {
        protected override void OnBindingContextChanged()
        {
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
