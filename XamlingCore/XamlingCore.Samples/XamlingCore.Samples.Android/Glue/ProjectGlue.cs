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

namespace XamlingCore.Samples.Droid.Glue
{
    public class ProjectGlue : DroidGlue
    {
        public override void Init()
        {
            base.Init();
            Container = Builder.Build();
        }
    }
}