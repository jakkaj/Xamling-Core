using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using XamlingCore.Portable.Contract.Glue;
using XamlingCore.Portable.Contract.Localisation;
using XamlingCore.Portable.Contract.Services;
using XamlingCore.Portable.Service.Localisation;
using XamlingCore.Portable.Service.Network;

namespace XamlingCore.Portable.Glue
{
    public class DefaultXCoreModule : Module
    {
        public static CultureInfo CultureInfo { get; set; }
        public static ResourceManager ResourceManager { get; set; }

        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(_ => new LocalisationService(CultureInfo, ResourceManager)).As<ILocalisationService>().SingleInstance();

            builder.RegisterType<NetworkService>().As<INetworkService>().SingleInstance();


            base.Load(builder);
        }
    }
}
