using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XamlingCore.XamarinThings.Utility
{
    public static class XDevice
    {
        public static void OnPlatform(Action iOS = null, Action Android = null, Action WindowsPhone = null, Action WindowsUniversal = null)
        {
            Device.OnPlatform(iOS, Android, WindowsPhone);

            if (WindowsUniversal != null && Device.OS == TargetPlatform.Windows)
            {
                WindowsUniversal();
            }
        }
    }
}
