using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Xamarin.Forms;
using XamlingCore.Portable.View.ViewModel;
using XamlingCore.XamarinThings.Content.MasterDetail;
using XamlingCore.XamarinThings.Content.Navigation;
using XamlingCore.XamarinThings.Frame;
using Module = Autofac.Module;

namespace XamlingCore.XamarinThings.Glue
{
    public class XamarinGlue : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<XFrameManager>().AsImplementedInterfaces();
            builder.RegisterType<XRootFrame>().AsSelf();

            builder.RegisterType<XMasterDetailView>().AsSelf();
            builder.RegisterType<XMasterDetailViewModel>().AsSelf();

            builder.RegisterType<XNavigationPageView>().AsSelf();
            builder.RegisterType<XNavigationPageViewModel>().AsSelf();

            builder.RegisterType<XNavigationPageTypedView>().AsSelf();
            builder.RegisterGeneric(typeof (XNavigationPageTypedViewModel<>)).AsSelf();

            base.Load(builder);
        }
    }
}
