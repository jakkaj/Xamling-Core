using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using XamlingCore.iOS.Glue;
using XamlingCore.Portable.Contract.Glue;
using XamlingCore.Portable.Glue;
using XamlingCore.Portable.View.ViewModel.Base;
using XamlingCore.Samples.iOS.Glue.Modules;

namespace XamlingCore.Samples.iOS.Glue
{
   public  class ProjectGlue : iOSGlue
    {
       public override void Init()
       {
           base.Init(); //ensure you call this first so the builder and container are available

           //do your project registrations here.
           Builder.RegisterModule<ViewModelModule>();
           Builder.RegisterModule<ViewsModule>();
           Builder.RegisterModule<FramesModelModule>();
           Container = Builder.Build();
       }


    }
}