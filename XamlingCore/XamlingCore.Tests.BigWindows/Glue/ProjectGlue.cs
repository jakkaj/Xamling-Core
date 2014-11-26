using Autofac;
using XamlingCore.Portable.Contract.Infrastructure.LocalStorage;
using XamlingCore.Portable.Contract.Network;
using XamlingCore.Portable.Glue;
using XamlingCore.Portable.Workflow.Glue;
using XamlingCore.Tests.BigWindows.Impl;
using XamlingCore.Windows8.Implementations;

namespace XamlingCore.Tests.BigWindows.Glue
{
    public class ProjectGlue : GlueBase
    {
        public override void Init()
        {
            base.Init();
            Builder.RegisterModule<DefaultXCoreModule>();
            //Builder.RegisterModule<DefaultWorkflowModule>();

            Builder.RegisterType<WinMockDeviceNetworkStatus>().As<IDeviceNetworkStatus>().SingleInstance();
            Builder.RegisterType<LocalStorageWindows8>().As<ILocalStorage>().SingleInstance();

            Container = Builder.Build();

        }
    }
}