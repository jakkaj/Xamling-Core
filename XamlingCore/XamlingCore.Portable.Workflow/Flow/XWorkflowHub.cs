using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XamlingCore.Portable.Contract.Network;

namespace XamlingCore.Portable.Workflow.Flow
{
    public class XWorkflowHub
    {
        private readonly IDeviceNetworkStatus _networkStatus;

        public XWorkflowHub(IDeviceNetworkStatus networkStatus)
        {
            _networkStatus = networkStatus;
        }

        readonly List<XFlow> _flows = new List<XFlow>(); 

        public XFlow AddFlow(string flowId, string friendlyName)
        {
            var f = new XFlow(_networkStatus).Setup(flowId, friendlyName);
            
            _flows.Add(f);

            return f;
        }

        public async Task<XFlow> Start(string flowId, Guid id)
        {
            var f = _flows.FirstOrDefault(_ => _.FlowId == flowId);
            
            if (f == null)
            {
                throw new NullReferenceException("Could not find flow with flowId: " + flowId);
            }

            await f.Start(id);

            return f;
        }

        public List<XFlow> GetInProgressFlow()
        {
            return _flows.Where(item => item.InProgressItems.Count > 0).ToList();
        }
    }
}
