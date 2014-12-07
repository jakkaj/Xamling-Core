using System;
using System.Collections.Generic;
using System.Text;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using XamlingCore.Portable.Contract.Device.Service;

namespace XamlingCore.iOS.Services
{
    public class DeviceUtilityService : IDeviceUtilityService
    {
        public void ShowInExternalBrowser(string url)
        {
            UIApplication.SharedApplication.OpenUrl(new NSUrl(url));
        }
    }
}
