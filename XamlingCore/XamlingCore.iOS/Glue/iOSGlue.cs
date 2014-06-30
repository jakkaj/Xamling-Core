using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
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
            
            Builder.RegisterModule<DefaultiOSModule>();
            Builder.RegisterModule<XamarinGlue>();
            Builder.RegisterModule<DefaultXCoreModule>();
        }
    }
}