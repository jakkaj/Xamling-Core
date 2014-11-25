using Autofac;
using XamlingCore.Portable.Glue;

namespace XamlingCore.Tests.BigWindows.Glue
{
    public class ProjectGlue : GlueBase
    {
        public override void Init()
        {
            base.Init();
            Builder.RegisterModule<DefaultXCoreModule>();
            Container = Builder.Build();

        }
    }
}