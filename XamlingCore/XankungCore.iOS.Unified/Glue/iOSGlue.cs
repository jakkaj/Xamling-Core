using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using Foundation;
using UIKit;
using XamlingCore.iOS.Glue.Modules;
using XamlingCore.Portable.Glue;
using XamlingCore.XamarinThings.Glue;

namespace XamlingCore.iOS.Glue
{
    public class iOSGlue : GlueBase 
    {
        public override void Init()
        {
            base.Init();
            Builder.RegisterModule<DefaultXCoreModule>();
            Builder.RegisterModule<DefaultiOSModule>();
            Builder.RegisterModule<XamarinGlue>();
            
        }
    }
}