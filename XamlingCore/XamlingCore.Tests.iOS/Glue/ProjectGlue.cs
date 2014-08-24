using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using XamlingCore.iOS.Glue;
using XamlingCore.Portable.Glue.Glue;

namespace XamlingCore.Tests.iOS.Glue
{
    public class ProjectGlue : iOSGlue
    {
        public override void Init()
        {
            base.Init();

            

            

            Container = Builder.Build();

        }
    }
}