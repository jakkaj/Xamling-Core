using System;
using Windows.Networking.Connectivity;
using XamlingCore.Portable.Contract.Network;
using XamlingCore.Portable.Model.Network;

namespace XamlingCore.UWP.Implementations
{
    public class Windows8DeviceNetworkStatus : IDeviceNetworkStatus
    {
        public event EventHandler NetworkChanged;

        public Windows8DeviceNetworkStatus()
        {
            NetworkInformation.NetworkStatusChanged+=NetworkInformation_NetworkStatusChanged;
        }

        void NetworkInformation_NetworkStatusChanged(object sender)
        {
            if (NetworkChanged != null)
            {
                NetworkChanged(this, EventArgs.Empty);
            }
        }

        public bool QuickNetworkCheck()
        {
            var i = NetworkInformation.GetInternetConnectionProfile();

            if (i == null)
            {
                return false;
            }

            var cLevel = i.GetNetworkConnectivityLevel();

            return cLevel == NetworkConnectivityLevel.InternetAccess;

        }

        public XNetworkType NetworkCheck()
        {
            var i = NetworkInformation.GetInternetConnectionProfile();

            if (i == null)
            {
                return XNetworkType.None;
            }

            var cLevel = i.GetNetworkConnectivityLevel();

            if (cLevel != NetworkConnectivityLevel.InternetAccess)
            {
                return XNetworkType.None;
            }

            var cost = i.GetConnectionCost();

            if (cost.NetworkCostType == NetworkCostType.Fixed || cost.NetworkCostType == NetworkCostType.Unrestricted)
            {
                return XNetworkType.WiFi;
            }

            return XNetworkType.Cellular;
        }
    }
}
