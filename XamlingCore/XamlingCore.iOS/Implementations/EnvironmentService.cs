using System;
using System.Collections.Generic;
using System.Text;
using MonoTouch.UIKit;

namespace XamlingCore.iOS.Implementations
{
    public class EnvironmentService : IEnvironmentService
    {
        public string GetOSVersion()
        {
            return UIDevice.CurrentDevice.SystemName + " " + UIDevice.CurrentDevice.SystemVersion;
        }
    }
}
