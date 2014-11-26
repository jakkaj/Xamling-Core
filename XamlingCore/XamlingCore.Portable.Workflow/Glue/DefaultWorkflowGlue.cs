using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using XamlingCore.Portable.Workflow.Flow;

namespace XamlingCore.Portable.Workflow.Glue
{
    public class DefaultWorkflowModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<XWorkflowHub>().AsSelf();
            base.Load(builder);
        }
    }
}
