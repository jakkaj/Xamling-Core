using Android.App;
using Android.Content.Res;
using Android.Content.PM;
using System;

using XamlingCore.Portable.Contract.Device;
using XamlingCore.Portable.Model.Orientation;


namespace XamlingCore.Droid.Implementations
{
    public class OrientationSensor : IOrientationSensor
    {
        public event EventHandler OrientationChanged;

        public XPageOrientation Orientation { get; private set; }

        public bool UpsideDown { get; private set; }

        public OrientationSensor()
        {
            _orientationUpdated();
        }

        public void OnRotated()
        {
            if (_orientationUpdated())
            {
                if (OrientationChanged != null)
                {
                    OrientationChanged(this, EventArgs.Empty);
                }
            }
        }

        //public override void onConfigurationChanged(Configuration newConfig)
        //{

        //}

        public bool _orientationUpdated()
        {
            //var e = Android.Content.Res.Orientation.Portrait;
            //var o = Resources.System.Configuration.Orientation;

            //Android.App.Activity

            //Android.Content.Res.Configuration.

            //object windowsService = Application.Context.GetSystemService(Android.App.Activity.WindowService);

            //createDisplayContext(Display)

            //windowsService.DefaultDisplay.Rotation;


            //Android.Content.PM.ConfigurationInfo.;

            //ActivityManager result = (ActivityManager)Application.Context.GetSystemService(Application.Context.ACTIVITY_SERVICE);

            //result.

            //getResources().getConfiguration().orientation         


            return false;

            //XPageOrientation _orientation = Orientation;
            //if (UIDevice.CurrentDevice.Orientation == UIDeviceOrientation.LandscapeLeft ||
            //    UIDevice.CurrentDevice.Orientation == UIDeviceOrientation.LandscapeRight)
            //{
            //    _orientation = XPageOrientation.Landscape;
            //}
            //if (UIDevice.CurrentDevice.Orientation == UIDeviceOrientation.Portrait ||
            //    UIDevice.CurrentDevice.Orientation == UIDeviceOrientation.PortraitUpsideDown)
            //{
            //    _orientation = XPageOrientation.Portrait;
            //}

            //UpsideDown = UIDevice.CurrentDevice.Orientation == UIDeviceOrientation.LandscapeRight
            //             || UIDevice.CurrentDevice.Orientation == UIDeviceOrientation.PortraitUpsideDown;

            //    Orientation = _orientation;
            //    return true;

        }
    }
}
