using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using XamlingCore.Portable.Glue;

namespace XamlingCore.Samples.iOS.Glue.Modules
{
    public class ViewModelModule:XAutofacModule
    {
        public ViewModelModule()
            : base(Assembly.Load("XamlingCore.Samples"), "ViewModel", true, false)
        {
            
        }
    }
}