using Autofac;
using XamlingCore.Portable.Contract.Network;
using XamlingCore.Portable.Glue;
using XamlingCore.Portable.Workflow.Glue;
using XamlingCore.Tests.BigWindows.Impl;

namespace XamlingCore.Tests.BigWindows.Glue
{
    public class ProjectGlue : GlueBase
    {
        public override void Init()
        {
            base.Init();
            Builder.RegisterModule<DefaultXCoreModule>();
            Builder.RegisterModule<DefaultWorkflowModule>();

            Builder.RegisterType<WinMockDeviceNetworkStatus>().As<IDeviceNetworkStatus>().SingleInstance();

            Container = Builder.Build();

        }
    }
}