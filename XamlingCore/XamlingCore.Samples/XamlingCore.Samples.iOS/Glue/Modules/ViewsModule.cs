using System.Reflection;
using Autofac;
using XamlingCore.Portable.Glue;
using XamlingCore.Portable.Glue.Glue;
using XamlingCore.Samples.XCore;

namespace XamlingCore.Samples.iOS.Glue.Modules
{
    public class ViewsModule : XAutofacModule
    {
        public ViewsModule()
            : base(Assembly.Load("XamlingCore.Samples"), "View", true, false)
        {
            
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<RootFrame>().AsImplementedInterfaces().AsSelf();
            base.Load(builder);
        }
    }
}
