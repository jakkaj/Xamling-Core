using Android.App;
using Android.Content;
using Android.Net;
using Java.Net;
using System;
using XamlingCore.Droid.Implementations.Helpers;
using XamlingCore.Portable.Contract.Network;
using XamlingCore.Portable.Model.Network;

namespace XamlingCore.Droid.Implementations
{
    public class DeviceNetworkStatus : IDeviceNetworkStatus
    {
        public event EventHandler NetworkChanged;
        ConnectivityManager _connectivity;
        NetworkChangesReceiver _network;

        //http://www.hasaltaiar.com.au/detecting-network-connectivity-status-changes-on-android-monoforandroid/
        //http://developer.xamarin.com/recipes/android/networking/networkinfo/detect_network_connection/

        public DeviceNetworkStatus()
        {
            //String link = "http://www.google.com";
            //URL url = new URL(link);
            //HttpURLConnection conn = (HttpURLConnection)url.OpenConnection();
            //conn.Connect();
            //conn.Disconnect();

            _connectivity = (ConnectivityManager)Application.Context.GetSystemService(Android.App.Activity.ConnectivityService);
            _network = new NetworkChangesReceiver();
            _network.NetworkStatusChanged += NetworkStatusChanged;
            Application.Context.RegisterReceiver(_network, new IntentFilter(ConnectivityManager.ConnectivityAction));
        }

        private void NetworkStatusChanged(object sender, EventArgs e)
        {
            if (NetworkChanged != null)
            {
                NetworkChanged(this, EventArgs.Empty);
            }
        }

        public XNetworkType NetworkCheck()
        {
            if (_connectivity != null)
            {
                var active = _connectivity.ActiveNetworkInfo;
                //var t2 = _connectivity.GetAllNetworkInfo();
                //var t3 = _connectivity.GetNetworkInfo(ConnectivityType.Mobile);


                if (active != null)
                {
                    switch (active.Type)
                    {
                        case ConnectivityType.Mobile:
                            return XNetworkType.Cellular;
                        case ConnectivityType.Wifi:
                            return XNetworkType.WiFi;
                    }
                }

                /* Included this because active is always null in tests */
                var mobileState = _connectivity.GetNetworkInfo(ConnectivityType.Mobile).GetState();
                if (mobileState != null)
                {
                    if (mobileState.ToString() == "CONNECTED")
                        return XNetworkType.Cellular;
                }

                var wifiState = _connectivity.GetNetworkInfo(ConnectivityType.Wifi).GetState();
                if (wifiState != null)
                {
                    if (wifiState.ToString() == "CONNECTED")
                        return XNetworkType.WiFi;
                }

                return XNetworkType.None;
            }

            return XNetworkType.Unknown;
        }

        public bool QuickNetworkCheck()
        {
            var activeConnection = _connectivity.ActiveNetworkInfo;
            return ((activeConnection != null) && activeConnection.IsConnected);            
        }

        //Required?
        //private void Cleanup()
        //{        
        //    if (_network != null)
        //    {
        //        _network.NetworkStatusChanged -= NetworkStatusChanged;
        //        Application.Context.UnregisterReceiver(_network);
        //        _network.Dispose();
        //    }
        //}
    }
}
