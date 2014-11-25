using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using XamlingCore.Portable.Model.Contract;
using XamlingCore.Portable.Workflow.Contract;

namespace XamlingCore.Portable.Workflow.Flow
{
    public class XWorkflowHub
    {
        private readonly ILifetimeScope _scope;

        public XWorkflowHub(ILifetimeScope scope)
        {
            _scope = scope;
        }

        public async Task StartWorkflow<TFlowType, TEntityType>(TEntityType entity)
            where TFlowType : IXFlow<TEntityType> 
            where TEntityType : IEntity
        {
            var flow = _scope.Resolve<TFlowType>();
            await flow.Add(entity);
        }
    }
}
