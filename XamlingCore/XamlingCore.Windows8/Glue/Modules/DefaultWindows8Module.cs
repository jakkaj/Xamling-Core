using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Autofac;
using XamlingCore.Portable.Contract.Device;
using XamlingCore.Portable.Contract.Infrastructure.LocalStorage;
using XamlingCore.Portable.Contract.Network;
using XamlingCore.Portable.Contract.UI;
using XamlingCore.Windows8.Implementations;
using Module = Autofac.Module;

namespace XamlingCore.Windows8.Glue.Modules
{
    public class DefaultWindows8Module : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<LocalStorageWindows8>().As<ILocalStorage>().SingleInstance();
            builder.RegisterType<Windows8Dispatcher>().As<IDispatcher>().SingleInstance();
            builder.RegisterType<OrientationSensor>().As<IOrientationSensor>();
            builder.RegisterType<Windows8DeviceNetworkStatus>().As<IDeviceNetworkStatus>();

            builder.RegisterAssemblyTypes(typeof(DefaultWindows8Module).GetTypeInfo().Assembly)
           .Where(_ => _.FullName.Contains("Service"))
           .AsImplementedInterfaces()
           .SingleInstance();

            base.Load(builder);
        }
    }
}
