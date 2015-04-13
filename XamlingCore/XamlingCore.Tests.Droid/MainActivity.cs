using System.Reflection;
using Android.App;
using Android.OS;
using Android.Content.PM; /* Stops app reload during orientation change */
using Xamarin.Android.NUnitLite;
using XamlingCore.Droid.Implementations;
using System.Threading.Tasks;
using System;
using XamlingCore.Droid.Services;
using Xamarin.Forms;


namespace XamlingCore.Tests.Droid
{
    [Activity(Label = "XamlingCore.Tests.Droid", MainLauncher = true, Icon = "@drawable/icon", ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize)]
    public class MainActivity : TestSuiteActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            // tests can be inside the main assembly
            AddTest(Assembly.GetExecutingAssembly());
            // or in any reference assemblies
            // AddTest (typeof (Your.Library.TestClass).Assembly);

            // Once you called base.OnCreate(), you cannot add more assemblies.
            base.OnCreate(bundle);

            //InnerTest();
        }

        private async void InnerTest()
        {
            await Task.Delay(1000);

            //DroidDispatcher dd = new DroidDispatcher();
            //dd.Invoke(new Action(() => Console.WriteLine("A test 1234")));


            //DeviceUtilityService d = new DeviceUtilityService();
            //d.ShowInExternalBrowser("http://www.xamling.net");


            //MotionSensor m = new MotionSensor();
            //m.Start();


            //OrientationSensor o = new OrientationSensor();
            //o._orientationUpdated();

            //DeviceNetworkStatus d = new DeviceNetworkStatus();
            //var z = d.NetworkCheck();
            //var i = d.QuickNetworkCheck();

            //EnvironmentService e = new EnvironmentService();
            //Console.WriteLine("Android version: " + e.GetOSVersion());
            //Console.WriteLine("Android width: " + e.GetScreenWidth());
            //Console.WriteLine("Android height: " + e.GetScreenHeight());
            //Console.WriteLine("Android scale: " + e.GetScreenScale());
            //Console.WriteLine("Android app v: " + e.GetAppVersion());
        }
    }
}

