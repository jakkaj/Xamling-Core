using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Xamarin.Forms.Platform.Android;
using XamlingCore.Samples.Droid.Glue;
using XamlingCore.Samples.Views.Root.MasterDetailRoot;

namespace XamlingCore.Samples.Droid
{
    [Activity(Label = "XamlingCore.Samples.Droid", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : FormsApplicationActivity
    {
        int count = 1;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            global::Xamarin.Forms.Forms.Init(this, bundle);
            var xapp = new App();
            xapp.Init<RootMasterDetailViewModel, ProjectGlue>();
            LoadApplication(xapp);
        }
    }
}

