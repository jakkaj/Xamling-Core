using System.Reflection;
using Autofac;
using XamlingCore.NET.Implementations;
using XamlingCore.Portable.Contract.Device;
using XamlingCore.Portable.Contract.Device.Service;
using XamlingCore.Portable.Contract.Downloaders;
using XamlingCore.Portable.Contract.Infrastructure.LocalStorage;
using XamlingCore.Portable.Contract.Network;
using XamlingCore.Portable.Contract.Services;
using XamlingCore.Portable.Contract.UI;
using Module = Autofac.Module;

namespace XamlingCore.NET.Glue
{
    public class DefaultNETModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<LocalStorage>().As<ILocalStorage>().SingleInstance();
            base.Load(builder);
        }
    }
}
