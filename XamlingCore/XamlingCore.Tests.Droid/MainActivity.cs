using System.Reflection;
using Android.App;
using Android.OS;
using Xamarin.Android.NUnitLite;
using XamlingCore.Droid.Implementations;
using System.Threading.Tasks;

namespace XamlingCore.Tests.Droid
{
    [Activity(Label = "XamlingCore.Tests.Droid", MainLauncher = true, Icon = "@drawable/icon")]
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
            //await Task.Delay(5000);
            //DeviceNetworkStatus d = new DeviceNetworkStatus();
            //var i = d.NetworkCheck();
        }
    }
}

