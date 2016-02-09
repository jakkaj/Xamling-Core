using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Autofac;
using XamlingCore.Portable.Contract.Device;
using XamlingCore.Portable.Contract.Device.Service;
using XamlingCore.Portable.Contract.Downloaders;
using XamlingCore.Portable.Contract.Infrastructure.LocalStorage;
using XamlingCore.Portable.Contract.Localisation;
using XamlingCore.Portable.Contract.Network;
using XamlingCore.Portable.Contract.Services;
using XamlingCore.Portable.Contract.UI;
using XamlingCore.Portable.Net.Downloaders;
using XamlingCore.Windows8.Implementations;
using XamlingCore.Windows8.Implementations.Helpers;
using XamlingCore.Windows8.Navigation;
using XamlingCore.XamarinThings.Contract;

using Module = Autofac.Module;
using XamlingCore.Portable.Contract.Helpers;

namespace XamlingCore.Windows8.Glue.Modules
{
    public class DefaultWindows8Module : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<Windows8ViewResolver>().As<IViewResolver>();
            builder.RegisterType<LocalStorageWindows8>().As<ILocalStorage>().SingleInstance();
            builder.RegisterType<Windows8Dispatcher>().As<IDispatcher>().SingleInstance();
            builder.RegisterType<OrientationSensor>().As<IOrientationSensor>();
            builder.RegisterType<Windows8DeviceNetworkStatus>().As<IDeviceNetworkStatus>();
            builder.RegisterType<Zip>().As<IZip>();
            builder.RegisterAssemblyTypes(typeof(DefaultWindows8Module).GetTypeInfo().Assembly)
           .Where(_ => _.FullName.Contains("Service"))
           .AsImplementedInterfaces()
           .SingleInstance();



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
