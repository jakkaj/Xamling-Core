using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Core;
using XamlingCore.Windows8.Glue;

namespace XamlingCore.Samples.Windows8.Glue
{
    public class ProjectGlue : Windows8Glue
    {
        public override void Init()
        {
            base.Init();

            Builder.RegisterAssemblyTypes(typeof(MainPage).GetTypeInfo().Assembly).Where(_ => _.FullName.Contains("View")).AsSelf();

           // Builder.RegisterType<WorkflowExamples>();
            Container = Builder.Build();
        }
    }
}
