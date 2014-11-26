using System.Collections.Generic;
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
        public XFlow AddFlow()
        {
            var f = new XFlow(_networkStatus);
            _flows.Add(f);

            return f;
        }
    }
}
