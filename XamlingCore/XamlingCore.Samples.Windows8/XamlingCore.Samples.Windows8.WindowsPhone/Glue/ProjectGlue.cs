using System.Reflection;
using Autofac;
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
