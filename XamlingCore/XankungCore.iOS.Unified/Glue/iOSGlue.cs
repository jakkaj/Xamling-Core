using Autofac;
using XamlingCore.iOS.Unified.Glue.Modules;
using XamlingCore.Portable.Glue;
using XamlingCore.XamarinThings.Glue;

namespace XamlingCore.iOS.Unified.Glue
{
    public class iOSGlue : GlueBase 
    {
        public override void Init()
        {
            base.Init();
            Builder.RegisterModule<DefaultXCoreModule>();
            Builder.RegisterModule<DefaultiOSModule>();
            Builder.RegisterModule<XamarinGlue>();
            
        }
    }
}