using XamlingCore.Samples.UWP.Native.View;
using XamlingCore.Windows8.Glue;
using XamlingCore.Windows8.Shared.Glue;

namespace XamlingCore.Samples.UWP.Native.Glue
{
  
        public class ProjectGlue : UWPGlue
        {
        public override void Init()
        {
            base.Init();

            XUWPCoreAutoRegistration.RegisterAssembly(Builder, typeof(HomeViewModel));
            //XCoreAutoRegistration.RegisterAssembly(Builder, typeof(ProjectGlue));

            // Builder.RegisterType<WorkflowExamples>();
            Container = Builder.Build();
        }
    }
    
}
