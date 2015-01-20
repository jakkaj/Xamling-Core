using Foundation;
using UIKit;
using XamlingCore.Portable.Contract.Device.Service;

namespace XamlingCore.iOS.Unified.Services
{
    public class DeviceUtilityService : IDeviceUtilityService
    {
        public void ShowInExternalBrowser(string url)
        {
            UIApplication.SharedApplication.OpenUrl(new NSUrl(url));
        }
    }
}
