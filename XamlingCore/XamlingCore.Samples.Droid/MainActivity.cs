using System;
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
using Autofac;
using Xamarin.Forms.Platform.Android;
using XamlingCore.Portable.Contract.Device.Location;
using XamlingCore.Portable.Data.Glue;
using XamlingCore.Samples.Droid.Glue;
using XamlingCore.Samples.Views.Root.MasterDetailRoot;

namespace XamlingCore.Samples.Droid
{
    [Activity(Label = "XamlingCore.Samples.Droid", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : FormsApplicationActivity
    {

        readonly string[] PermissionsLocation =
        {
            Manifest.Permission.AccessCoarseLocation,
            Manifest.Permission.AccessFineLocation
        };

        int count = 1;

        const int RequestLocationId = 0;


        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            global::Xamarin.Forms.Forms.Init(this, bundle);
            var xapp = new App();
            xapp.Init<RootMasterDetailViewModel, ProjectGlue>();
            LoadApplication(xapp);


            AskLocationPermissions();

        }

        void _initLocation()
        {
            var l = ContainerHost.Container.Resolve<ILocationTrackingSensor>();
            l.Init();
            l.StartTracking();
        }

        public void AskLocationPermissions()
        {
            //if ((int)Build.VERSION.SdkInt < 23)
            //{
            //    return;
            //}

            const string permission = Manifest.Permission.AccessFineLocation;

            if (ContextCompat.CheckSelfPermission(this, permission) == (int)Permission.Granted)
            {
                _initLocation();
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

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            _initLocation();
        }
    }
}

