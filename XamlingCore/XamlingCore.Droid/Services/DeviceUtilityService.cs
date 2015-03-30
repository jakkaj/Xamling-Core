using Android.App;
using Android.Content;
using Java.Lang;
using XamlingCore.Portable.Contract.Device.Service;

namespace XamlingCore.Droid.Services
{
    public class DeviceUtilityService : IDeviceUtilityService
    {
        public void ShowInExternalBrowser(string url)
        {
            try
            {
                var intent = new Intent(Intent.ActionView, Android.Net.Uri.Parse(url));
                intent.SetFlags(ActivityFlags.NewTask);            
                Application.Context.StartActivity(intent);

            } catch(Exception e){ }
        }
    }
}
