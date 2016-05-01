using System;
using System.Threading.Tasks;
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using Xamarin.Forms.Platform.Android;
using XamlingCore.Samples.Droid.Glue;
using XamlingCore.Samples.Views.Root.MasterDetailRoot;

namespace XamlingCore.Samples.Droid
{
    [Activity(Label = "XamlingCore.Samples.Droid", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : FormsApplicationActivity
    {
        int count = 1;

        readonly string[] PermissionsLocation =
       {
          Manifest.Permission.AccessCoarseLocation,
          Manifest.Permission.AccessFineLocation
       };

        const int RequestLocationId = 0;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            global::Xamarin.Forms.Forms.Init(this, bundle);
            var xapp = new App();
            xapp.Init<RootMasterDetailViewModel, ProjectGlue>();
            LoadApplication(xapp);

        }

        public async Task AskLocationPermissions()
        {
            if ((int) Build.VERSION.SdkInt < 23)
            {
                return;
            }

            const string permission = Manifest.Permission.AccessFineLocation;

            if (ContextCompat.CheckSelfPermission(this, permission) == (int)Permission.Granted)
            {
                return;
            }

            if (ActivityCompat.ShouldShowRequestPermissionRationale(this, permission))
            {
                //Explain to the user why we need to read the contacts
                Snackbar.Make(this.FindViewById(Android.Resource.Id.Content), "Location access is required to show nearby stores.",
                    Snackbar.LengthIndefinite)
                    .SetAction("OK", v => RequestPermissions(PermissionsLocation, RequestLocationId))
                    .Show();

                return;
            }

            RequestPermissions(PermissionsLocation, RequestLocationId);
        }
    }
}

