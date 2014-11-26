using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XamlingCore.Portable.Contract.Infrastructure.LocalStorage;
using XamlingCore.Portable.Contract.Network;
using XamlingCore.Portable.Contract.Serialise;

namespace XamlingCore.Portable.Workflow.Flow
{
    public class XWorkflowHub
    {
        private readonly IDeviceNetworkStatus _networkStatus;
        private readonly IEntitySerialiser _entitySerialiser;
        private readonly ILocalStorage _localStorage;

        public XWorkflowHub(IDeviceNetworkStatus networkStatus, IEntitySerialiser entitySerialiser, ILocalStorage localStorage)
        {
            _networkStatus = networkStatus;
            _entitySerialiser = entitySerialiser;
            _localStorage = localStorage;
        }

        readonly List<XFlow> _flows = new List<XFlow>(); 

        public XFlow AddFlow(string flowId, string friendlyName)
        {
            if (GetFlow(flowId) != null)
            {
                throw new InvalidOperationException(string.Format("Flow already created with flowId: {0}", flowId));
            }

            var f = new XFlow(_networkStatus, _entitySerialiser, _localStorage).Setup(flowId, friendlyName);
            
            _flows.Add(f);

            return f;
        }

        public XFlow GetFlow(string flowId)
        {
            return _flows.FirstOrDefault(_ => _.FlowId == flowId);
        }

        public XFlowState GetFlowState(string flowId, Guid id)
        {
            var flow = GetFlow(flowId);

            if (flow == null)
            {
                return null;
            }

            return flow.GetState(id);
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

        public async Task<List<XFlow>> GetInProgressFlow()
        {
            var l = new List<XFlow>();

            foreach (var item in _flows)
            {

                if ((await item.GetInProgressItems()).Count > 0)
                {
                    l.Add(item);
                }
            }
            return l;
        }
    }
}
