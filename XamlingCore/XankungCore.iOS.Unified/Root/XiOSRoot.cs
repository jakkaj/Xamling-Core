using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;

namespace XamlingCore.iOS.Root
{
    public static class XiOSRoot
    {
        public static UIViewController RootViewController { get; set; }
        public static UIWindow RootWindow { get; set; }
    }
}