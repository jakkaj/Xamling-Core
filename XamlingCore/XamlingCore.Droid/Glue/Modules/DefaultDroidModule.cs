using Android.App;
using Autofac;
using XamlingCore.Droid.Implementations;
using XamlingCore.Droid.Implementations.Helpers;
using XamlingCore.Droid.Navigation;
using XamlingCore.Droid.Services;
using XamlingCore.Portable.Contract.Device.Service;
using XamlingCore.Portable.Contract.Downloaders;
using XamlingCore.Portable.Contract.Infrastructure.LocalStorage;
using XamlingCore.Portable.Contract.Network;
using XamlingCore.Portable.Contract.Services;
using XamlingCore.Portable.Contract.UI;
using XamlingCore.Portable.Net.Service;

namespace XamlingCore.Droid.Glue.Modules
{
    public class DefaultDroidModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //builder.RegisterType<RootViewController>().AsSelf().SingleInstance();

            builder.RegisterType<LocalStorage>().As<ILocalStorage>().SingleInstance();
            builder.RegisterType<LoadStatusService>().As<ILoadStatusService>().SingleInstance();
            builder.RegisterType<EnvironmentService>().As<IEnvironmentService>().SingleInstance();

            
            builder.Register(_ => new DroidDispatcher()).As<IDispatcher>().SingleInstance(); //Needs testing -- Can also try like //builder.Register(_ => new DroidDispatcher(Android.App.Application.Context)).As<IDispatcher>().SingleInstance();

            builder.RegisterType<LocationTrackingSensor>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<DeviceNetworkStatus>().As<IDeviceNetworkStatus>().SingleInstance();            

            builder.RegisterType<HashHelper>().AsImplementedInterfaces();

            builder.RegisterType<MotionSensor>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<OrientationSensor>().AsImplementedInterfaces().SingleInstance(); /* Needs work */
            
            builder.RegisterType<DroidNativeHttpClientTransferrer>().As<IHttpTransferrer>().SingleInstance();

            //builder.RegisterType<iOSSimpleNativeHttpHttpTransfer>().As<ISimpleHttpTranferrer>().SingleInstance(); /* Probably not required, do last */

            builder.RegisterType<DeviceUtilityService>().As<IDeviceUtilityService>().SingleInstance();

            builder.RegisterType<DroidViewResolver>().AsImplementedInterfaces().SingleInstance(); /* Not required */

            base.Load(builder);
        }
    }
}