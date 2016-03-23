using Autofac;
using XamlingCore.Portable.Glue;
using XamlingCore.UWP.Glue.Modules;

namespace XamlingCore.UWP.Glue
{
    public class UWPGlue : GlueBase
    {
        public override void Init()
        {
            base.Init();
            Builder.RegisterModule<DefaultXCoreModule>();
            Builder.RegisterModule<DefaultNativeUWPModule>();
        }
    }
}
