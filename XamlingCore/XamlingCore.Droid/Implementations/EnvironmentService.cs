using Android.App;
using Android.Content.Res;
using XamlingCore.Portable.Contract.Services;

namespace XamlingCore.Droid.Implementations
{
    public class EnvironmentService : IEnvironmentService
    {
        public string GetAppVersion()
        {
            return Application.Context.PackageManager.GetPackageInfo(Application.Context.PackageName, 0).VersionName;
        }

        public string GetOSVersion()
        {
            return Android.OS.Build.VERSION.SdkInt + " " + Android.OS.Build.VERSION.Release + " (" + Android.OS.Build.VERSION.Codename + ")";
        }

        public double GetScreenWidth()
        {
            var px = Resources.System.DisplayMetrics.WidthPixels;
            return px;
            //return ConvertPixelsToDp(px); //Scaled pixels
        }

        public double GetScreenHeight()
        {
            var px = Resources.System.DisplayMetrics.HeightPixels;
            return px;
            //return ConvertPixelsToDp(px); //Scaled pixels
        }

        public float GetScreenScale()
        {
            return Resources.System.DisplayMetrics.Density; //TODO: This might need to be 1/density?
        }

        private int ConvertPixelsToDp(float pixelValue)
        {
            var dp = (int)((pixelValue) / Resources.System.DisplayMetrics.Density);
            return dp;
        }
    }
}
