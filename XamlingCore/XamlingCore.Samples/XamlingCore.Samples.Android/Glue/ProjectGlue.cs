using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using XamlingCore.Droid.Glue;
using XamlingCore.Platform.Shared.Glue;
using XamlingCore.Samples.Views.MasterDetailHome.Home;

namespace XamlingCore.Samples.Droid.Glue
{
    public class ProjectGlue : DroidGlue
    {
        public override void Init()
        {
            base.Init();

            XCoreAutoRegistration.RegisterAssembly(Builder, typeof(HomeViewModel));
            XCoreAutoRegistration.RegisterAssembly(Builder, typeof(ProjectGlue));

            Container = Builder.Build();
        }
    }
}