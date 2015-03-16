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
using Autofac;
using XamlingCore.Droid.Glue.Modules;
using XamlingCore.Portable.Glue;
using XamlingCore.XamarinThings.Glue;

namespace XamlingCore.Droid.Glue
{
    public class DroidGlue : GlueBase
    {
        public override void Init()
        {
            base.Init();
            Builder.RegisterModule<DefaultXCoreModule>();
            Builder.RegisterModule<DefaultDroidModule>();
            Builder.RegisterModule<XamarinGlue>();

        }
    }
}