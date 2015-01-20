using System;
using System.Collections.Generic;
using System.Text;
using UIKit;

namespace XamlingCore.iOS.Implementations
{
    public class EnvironmentService : IEnvironmentService
    {
        public string GetOSVersion()
        {
            return UIDevice.CurrentDevice.SystemName + " " + UIDevice.CurrentDevice.SystemVersion;
        }

        public int GetScreenWidth()
        {
            return (int) UIScreen.MainScreen.Bounds.Width;
        }

        public int GetScreenHeight()
        {
            return (int) UIScreen.MainScreen.Bounds.Height;
        }

        public float GetScreenScale()
        {
            return (float)UIScreen.MainScreen.Scale;
        }
    }
}
