using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using XamlingCore.Portable.Contract.Glue;
using XamlingCore.Portable.Glue;

namespace XamlingCore.Samples.iOS.Glue
{
   public  class ProjectGlue : GlueBase
    {
       public override void Init()
       {
           base.Init(); //ensure you call this first so the builder and container are available

           Builder.RegisterModule<DefaultXCoreModule>();
       }


    }
}