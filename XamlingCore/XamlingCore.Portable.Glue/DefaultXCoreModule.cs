using Autofac;
using XamlingCore.Portable.Contract.Downloaders;
using XamlingCore.Portable.Contract.Entities;
using XamlingCore.Portable.Contract.Localisation;
using XamlingCore.Portable.Contract.Navigation;
using XamlingCore.Portable.Contract.Repos.Base;
using XamlingCore.Portable.Contract.Serialise;
using XamlingCore.Portable.Contract.Services;
using XamlingCore.Portable.Data.Entities;
using XamlingCore.Portable.Data.Repos;
using XamlingCore.Portable.Data.Repos.Base;
using XamlingCore.Portable.Data.Serialise;
using XamlingCore.Portable.Net.Downloaders;
using XamlingCore.Portable.Service.Localisation;
using XamlingCore.Portable.Service.Location;
using XamlingCore.Portable.Service.Orientation;
using XamlingCore.Portable.Service.Settings;
using XamlingCore.Portable.View.Navigation;
using XamlingCore.Portable.Workflow.Glue;

namespace XamlingCore.Portable.Glue
{
    public class DefaultXCoreModule : Module
    {
       

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<LocalisationService>().As<ILocalisationService>().SingleInstance();
            
            builder.RegisterType<LocalStorageFileRepo>().As<IStorageFileRepo>().SingleInstance();

            builder.RegisterGeneric(typeof (EntityManager<>)).As(typeof (IEntityManager<>)).SingleInstance();
            builder.RegisterGeneric(typeof(EntityBucket<>)).As(typeof(IEntityBucket<>)).SingleInstance();

            builder.RegisterGeneric(typeof(XWebRepo<>)).As(typeof(IXWebRepo<>)).InstancePerDependency();

            //Core services
            builder.RegisterType<OrientationService>().As<IOrientationService>().SingleInstance();
            builder.RegisterType<GeneralSettingsService>().As<IGeneralSettingsService>().SingleInstance();
            builder.RegisterType<LocationService>().As<ILocationService>().SingleInstance();

            //Core utilities
            builder.RegisterType<JsonNETEntitySerialiser>().As<IEntitySerialiser>().SingleInstance();
            
            builder.RegisterType<SimpleHttpHttpTransfer>().As<ISimpleHttpTranferrer>().SingleInstance();

            builder.RegisterType<EntityCache>().As<IEntityCache>().SingleInstance();
            builder.RegisterType<MemoryCache>().As<IMemoryCache>().SingleInstance();

            builder.RegisterType<XNavigationService>().As<IXNavigation>();

            builder.RegisterType<HttpClientTransferrer>().As<IHttpTransferrer>().SingleInstance();

            builder.RegisterModule<DefaultWorkflowModule>();

            builder.RegisterType<LocalisedResourceReader>().As<ILocalisedResourceReader>();

            base.Load(builder);
        }
    }
}
