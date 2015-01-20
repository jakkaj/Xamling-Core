using System;
using XamlingCore.iOS.Unified.Implementations.Helpers;
using XamlingCore.Portable.Contract.Network;
using XamlingCore.Portable.Model.Network;

namespace XamlingCore.iOS.Unified.Implementations
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

        public XNetworkType NetworkCheck()
        {
            var internetStatus = Reachability.InternetConnectionStatus();
            switch (internetStatus)
            {
                case NetworkStatus.NotReachable:
                    return XNetworkType.None;
                case NetworkStatus.ReachableViaCarrierDataNetwork:
                    return XNetworkType.Cellular;
                case NetworkStatus.ReachableViaWiFiNetwork:
                    return XNetworkType.WiFi;
                default: return XNetworkType.Unknown;
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
