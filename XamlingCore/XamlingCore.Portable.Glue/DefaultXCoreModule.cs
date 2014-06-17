using System.Globalization;
using System.Resources;
using Autofac;
using XamlingCore.Portable.Contract.Cache;
using XamlingCore.Portable.Contract.Downloaders;
using XamlingCore.Portable.Contract.Localisation;
using XamlingCore.Portable.Contract.Navigation;
using XamlingCore.Portable.Contract.Repos;
using XamlingCore.Portable.Contract.Repos.Base;
using XamlingCore.Portable.Contract.Serialise;
using XamlingCore.Portable.Contract.Services;
using XamlingCore.Portable.Data.Cache;
using XamlingCore.Portable.Data.Repos;
using XamlingCore.Portable.Data.Repos.Base;
using XamlingCore.Portable.Data.Serialise;
using XamlingCore.Portable.Net.Downloaders;
using XamlingCore.Portable.Service.Localisation;
using XamlingCore.Portable.Service.Location;
using XamlingCore.Portable.Service.Network;
using XamlingCore.Portable.Service.Orientation;
using XamlingCore.Portable.View.Navigation;

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
            builder.RegisterType<LocalStorageFileRepo>().As<ILocalStorageFileRepo>().SingleInstance();

            //Core services
            builder.RegisterType<OrientationService>().As<IOrientationService>().SingleInstance();
            builder.RegisterType<LocationService>().As<ILocationService>().SingleInstance();

            //Core utilities
            builder.RegisterType<JsonNETEntitySerialiser>().As<IEntitySerialiser>().SingleInstance();

            builder.RegisterType<LocalStorageSettingsRepo>().As<ISettingsRepo>();

            //builder.RegisterType<LocationTrackingSensor>().As<ILocationTrackingSensor>().SingleInstance();

            
            
            builder.RegisterType<SimpleDownloader>().As<ISimpleDownloader>().SingleInstance();

            builder.RegisterType<EntityCache>().As<IEntityCache>().SingleInstance();


            builder.RegisterType<XNavigationService>().As<IXNavigation>().SingleInstance();

            base.Load(builder);
        }
    }
}
