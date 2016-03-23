using System.Reflection;
using Autofac;
using XamlingCore.Portable.Contract.Device;
using XamlingCore.Portable.Contract.Device.Service;
using XamlingCore.Portable.Contract.Downloaders;
using XamlingCore.Portable.Contract.Helpers;
using XamlingCore.Portable.Contract.Infrastructure.LocalStorage;
using XamlingCore.Portable.Contract.Localisation;
using XamlingCore.Portable.Contract.Network;
using XamlingCore.Portable.Contract.Services;
using XamlingCore.Portable.Contract.UI;
using XamlingCore.Portable.Net.Downloaders;
using XamlingCore.UWP.Contract;
using XamlingCore.UWP.Core;
using XamlingCore.UWP.Implementations;
using XamlingCore.UWP.Implementations.Helpers;
using XamlingCore.UWP.Navigation;
using LocalisedResourceReader = XamlingCore.Portable.Service.Localisation.LocalisedResourceReader;
using Module = Autofac.Module;


namespace XamlingCore.UWP.Glue.Modules
{
    public class DefaultNativeUWPModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<WindowsNativeViewResolver>().As<IUWPViewResolver>();

            builder.RegisterType<XUWPFrameManager>().As<IXUWPFrameManager>();
            builder.RegisterType<XUWPRootFrame>().AsSelf();


            builder.RegisterType<LocalStorageWindows8>().As<ILocalStorage>().SingleInstance();
            builder.RegisterType<Windows8Dispatcher>().As<IDispatcher>().SingleInstance();
            builder.RegisterType<OrientationSensor>().As<IOrientationSensor>();
            builder.RegisterType<Windows8DeviceNetworkStatus>().As<IDeviceNetworkStatus>();
            builder.RegisterType<Zip>().As<IZip>();

            builder.RegisterAssemblyTypes(typeof(DefaultNativeUWPModule).GetTypeInfo().Assembly)
           .Where(_ => _.FullName.Contains("Service"))
           .AsImplementedInterfaces()
           .SingleInstance();

            builder.RegisterAssemblyTypes(typeof (DefaultNativeUWPModule).GetTypeInfo().Assembly)
                .Where(_ => _.FullName.EndsWith("ViewModel")).AsSelf();

            builder.RegisterAssemblyTypes(typeof(DefaultNativeUWPModule).GetTypeInfo().Assembly)
                .Where(_ => _.FullName.EndsWith("View")).AsSelf();




            builder.RegisterType<EnvironmentService>().As<IEnvironmentService>().SingleInstance();
            builder.RegisterType<WindowsUniversalDispatcher>().As<IDispatcher>().SingleInstance();
            builder.RegisterType<LocationTrackingSensor>().AsImplementedInterfaces().SingleInstance();


            builder.RegisterType<LocalisedResourceReader>().As<ILocalisedResourceReader>();

            builder.RegisterType<HashHelper>().AsImplementedInterfaces();

            builder.RegisterType<MotionSensor>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<OrientationSensor>().AsImplementedInterfaces().SingleInstance();

            builder.RegisterType<HttpClientTransferrer>().As<IHttpTransferrer>().SingleInstance();
           // builder.RegisterType<iOSSimpleNativeHttpHttpTransfer>().As<ISimpleHttpTranferrer>().SingleInstance();

            builder.RegisterType<DeviceUtilityService>().As<IDeviceUtilityService>().SingleInstance();



            base.Load(builder);
        }
    }
}
