using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.System;
using XamlingCore.Portable.Contract.Device.Service;

namespace XamlingCore.Windows8.Implementations
{
    public class DeviceUtilityService : IDeviceUtilityService
    {
        public void ShowInExternalBrowser(string url)
        {
            Launcher.LaunchUriAsync(new Uri(url));
        }
    }
}
