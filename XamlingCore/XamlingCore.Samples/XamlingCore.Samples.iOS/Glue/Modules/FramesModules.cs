using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using XamlingCore.Portable.Glue.Glue;

namespace XamlingCore.Samples.iOS.Glue.Modules
{
    public class FramesModelModule : XAutofacModule
    {
        public FramesModelModule()
            : base(Assembly.Load("XamlingCore.Samples"), "Frame", true, false)
        {

        }
    }
}