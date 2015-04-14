using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

using Xamarin.Forms.Platform.Android;
using XamlingCore.Droid;
using XamlingCore.Samples.Droid.Annotations;
using XamlingCore.Samples.Views.Root.MasterDetailRoot;
using XamlingCore.Samples.Droid.Glue;
using XamlingCore.XamarinThings.Frame;

namespace XamlingCore.Samples.Droid
{
     [Activity(Label = "Xamling Core Droid Sample", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
    {
        
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            
            Xamarin.Forms.Forms.Init(this, bundle);
            var xapp = new App();
            xapp.Init<RootMasterDetailViewModel, ProjectGlue>();
            LoadApplication(xapp);
        }
    }
}

