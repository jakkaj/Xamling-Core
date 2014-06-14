using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XamlingCore.Portable.Contract.Network;
using XamlingCore.Portable.Contract.Services;
using XamlingCore.Portable.Messages.Network;
using XamlingCore.Portable.XamlingMessenger;

namespace XamlingCore.Portable.Service.Network
{
    public class NetworkService : INetworkService
    {
        private readonly IDeviceNetworkStatus _deviceNetworkStatus;

        public NetworkService(IDeviceNetworkStatus deviceNetworkStatus)
        {
            _deviceNetworkStatus = deviceNetworkStatus;
            _deviceNetworkStatus.NetworkChanged += _deviceNetworkStatus_NetworkChanged;
        }

        void _deviceNetworkStatus_NetworkChanged(object sender, EventArgs e)
        {
            new NetworkChangedMessage().Send();
        }

        public void RaiseProblemDownloading()
        {
            //new CouldNotContactServersErrorMessage().Send();
        }

        public bool CheckNetworkAvailable()
        {
            return _deviceNetworkStatus.QuickNetworkCheck();
        }
    }
}
