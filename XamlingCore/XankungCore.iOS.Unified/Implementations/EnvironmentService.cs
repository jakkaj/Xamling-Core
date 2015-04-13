using Foundation;
using UIKit;
using XamlingCore.Portable.Contract.Services;

namespace XamlingCore.iOS.Unified.Implementations
{
    public class EnvironmentService : IEnvironmentService
    {
        public string GetAppVersion()
        {
            var build = NSBundle.MainBundle.InfoDictionary["CFBundleVersion"].ToString();
            var version = NSBundle.MainBundle.InfoDictionary["CFBundleShortVersionString"].ToString();


            var versionString = string.Format("{0} ({1})", version, build);

            return versionString;
        }

        public string GetOSVersion()
        {
            return UIDevice.CurrentDevice.SystemName + " " + UIDevice.CurrentDevice.SystemVersion;
        }

        public int GetScreenWidth()
        {
            return (int) UIScreen.MainScreen.Bounds.Width;
        }

        public int GetScreenHeight()
        {
            return (int) UIScreen.MainScreen.Bounds.Height;
        }

        public float GetScreenScale()
        {
            return (float)UIScreen.MainScreen.Scale;
        }
    }
}
