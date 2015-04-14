using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Xamarin.Forms;
using XamlingCore.Samples.Windows8.Root;
using XamlingCore.Samples.Windows8.View;
using XamlingCore.Samples.Windows8.Workflow;
using XamlingCore.Windows8.Glue;
using XamlingCore.Windows8.Glue.Modules;
using XamlingCore.Windows8.Shared.Glue;

namespace XamlingCore.Samples.Windows8.Glue
{
  
        public class ProjectGlue : Windows8Glue
        {
            public override void Init()
            {
                base.Init();

                XCoreAutoRegistration.RegisterAssembly(Builder, typeof(HomeViewModel));
                XCoreAutoRegistration.RegisterAssembly(Builder, typeof(ProjectGlue));

                // Builder.RegisterType<WorkflowExamples>();
                Container = Builder.Build();
            }
        }
    
}
