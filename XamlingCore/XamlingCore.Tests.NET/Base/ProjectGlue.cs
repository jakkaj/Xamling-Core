using Autofac;
using XamlingCore.Portable.Contract.Repos;
using XamlingCore.Portable.Contract.Services;
using XamlingCore.Portable.Data.Glue;
using XamlingCore.Portable.Data.Security;
using XamlingCore.Portable.Glue;
using XamlingCore.Tests.NET.Security;

namespace XamlingCore.Tests.NET.Base
{
    public class ProjectGlue : GlueBase
    {
        public override void Init()
        {
            base.Init(); //ensure you call this first so the builder and container are available

            Builder.RegisterType<SecurityService>().As<ISecurityService>().SingleInstance();
            Builder.RegisterType<MockMemorySecurityRepo>().As<ISecurityRepo>().SingleInstance();
            

            Container = Builder.Build();
            ContainerHost.Container = Container;
        }


    }
}
