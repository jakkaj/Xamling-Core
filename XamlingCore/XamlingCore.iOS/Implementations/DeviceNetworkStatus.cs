using System;
using System.Collections.Generic;
using System.Text;
using XamlingCore.Portable.Contract.Network;

namespace XamlingCore.iOS.Implementations
{
    public class DeviceNetworkStatus : IDeviceNetworkStatus
    {
        public event EventHandler NetworkChanged;

        public DeviceNetworkStatus()
        {
            Reachability.ReachabilityChanged += Reachability_ReachabilityChanged;
        }

        void Reachability_ReachabilityChanged(object sender, EventArgs e)
        {
            if (NetworkChanged != null)
            {
                NetworkChanged(this, EventArgs.Empty);
            }
        }

        public bool QuickNetworkCheck()
        {
            var internetStatus = Reachability.InternetConnectionStatus();
            

            return internetStatus == NetworkStatus.ReachableViaCarrierDataNetwork ||
                   internetStatus == NetworkStatus.ReachableViaWiFiNetwork;
        }
    }
}
