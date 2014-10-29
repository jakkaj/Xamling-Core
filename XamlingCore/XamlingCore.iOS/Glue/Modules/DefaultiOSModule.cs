using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using XamlingCore.iOS.Implementations;
using XamlingCore.iOS.Implementations.Helpers;
using XamlingCore.iOS.Navigation;
using XamlingCore.Portable.Contract.Infrastructure.LocalStorage;
using XamlingCore.Portable.Contract.Network;
using XamlingCore.Portable.Contract.Services;
using XamlingCore.Portable.Contract.UI;
using XamlingCore.XamarinThings.Frame;

namespace XamlingCore.iOS.Glue.Modules
{
    public class DefaultiOSModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<LocalStorage>().As<ILocalStorage>().SingleInstance();
            builder.RegisterType<LoadStatusService>().As<ILoadStatusService>().SingleInstance();
            builder.RegisterType<EnvironmentService>().As<IEnvironmentService>().SingleInstance();
            builder.Register(_ => new iOSDispatcher(new NSObject())).As<IDispatcher>().SingleInstance();
            builder.RegisterType<LocationTrackingSensor>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<DeviceNetworkStatus>().As<IDeviceNetworkStatus>().SingleInstance();
            
            builder.RegisterType<iOSViewResolver>().AsImplementedInterfaces();
            
            builder.RegisterType<HashHelper>().AsImplementedInterfaces();

            base.Load(builder);
        }
    }
}