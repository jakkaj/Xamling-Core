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
using XamlingCore.Tests.Droid.Config;
using Autofac;
using XamlingCore.Portable.Contract.Downloaders;

namespace XamlingCore.Tests.Droid.Glue
{

    public class ProjectGlue : DroidGlue
    {
        public override void Init()
        {
            base.Init();

            Builder.RegisterType<TestTransferConfigService>().As<IHttpTransferConfigService>().SingleInstance();

            Container = Builder.Build();


        }
    }
}