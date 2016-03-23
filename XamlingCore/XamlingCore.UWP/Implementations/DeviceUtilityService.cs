using System;
using Windows.System;
using XamlingCore.Portable.Contract.Device.Service;

namespace XamlingCore.UWP.Implementations
{
    public class DeviceUtilityService : IDeviceUtilityService
    {
        public void ShowInExternalBrowser(string url)
        {
            Launcher.LaunchUriAsync(new Uri(url));
        }
    }
}
