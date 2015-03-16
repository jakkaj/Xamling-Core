using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

using Xamarin.Forms.Platform.Android;
using XamlingCore.Droid;
using XamlingCore.Samples.Views.Root.MasterDetailRoot;
using XamlingCore.Samples.Droid.Glue;
using XamlingCore.XamarinThings.Frame;

namespace XamlingCore.Samples.Droid
{
    [Activity(Label = "XamlingCore.Samples", MainLauncher = true)]
    public class MainActivity : AndroidActivity
    {
        private XDriodCore<XRootFrame, RootMasterDetailViewModel, ProjectGlue> xCore;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            Xamarin.Forms.Forms.Init(this, bundle);

            xCore = new XDriodCore<XRootFrame, RootMasterDetailViewModel, ProjectGlue>();

            var app = xCore.Init();

            LoadApplication(app);
        }
    }
}

