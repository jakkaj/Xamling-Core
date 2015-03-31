using Android.App;
using Android.Content.Res;
using Android.Content.PM;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;

using XamlingCore.Portable.Contract.Device;
using XamlingCore.Portable.Model.Orientation;
using XamlingCore.Droid.Implementations.Helpers;

namespace XamlingCore.Droid.Implementations
{
    //[Activity(Label = "OrientationSensor", ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.ScreenSize)]
    public class OrientationSensor : IOrientationSensor //Activity
    {


        public event EventHandler OrientationChanged;

        public XPageOrientation Orientation { get; private set; }

        public bool UpsideDown { get; private set; }

        public OrientationSensor()
        {
            //OnCreate(null);

            //var orientationIntent = new Intent(Application.Context, typeof(OrientationActivity));

            //var t = (OrientationActivity)orientationIntent.;


            //_orientationUpdated();
            //WindowManager windowManager = (WindowManager)getSystemService(WINDOW_SERVICE);
            // object windowsService = Application.Context.GetSystemService(Android.App.Activity.WindowService);
        }

        //protected override void OnCreate(Bundle bundle)
        //{

        //}

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

        //public override void OnConfigurationChanged(Configuration newConfig)
        //{
        //    Console.WriteLine("Orientation changed");
        //    base.OnConfigurationChanged(newConfig);            
        //}

        public bool _orientationUpdated()
        {
            //var e = Android.Content.Res.Orientation.Portrait;
            //var o = Resources.System.Configuration.Orientation;

            //Android.App.Activity

            //Android.Content.Res.Configuration.

            //-----------
            IWindowManager windowManager = Application.Context.GetSystemService(Context.WindowService).JavaCast<IWindowManager>();
            var d = windowManager.DefaultDisplay;
            

            var h = d.Height;
            var w = d.Width;
            var r = d.Rotation;
            var o = d.Orientation;

            Console.WriteLine("{0}x{1} @ {2}deg giving orientation of {3}", w, h, r, o);
            //-------------



            //Activity.GetResources().getConfiguration().orientation


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
