using System;
using System.Collections.Generic;
using System.Text;
using MonoTouch.UIKit;
using XamlingCore.iOS.Root;
using XamlingCore.Portable.Contract.Device;
using XamlingCore.Portable.Model.Orientation;

namespace XamlingCore.iOS.Implementations
{
    public class OrientationSensor : IOrientationSensor
    {
        public event EventHandler OrientationChanged;

        public XPageOrientation Orientation { get; private set; }

        public bool UpsideDown { get; private set; }

        public OrientationSensor()
        {
            _orientationUpdated();
        }

        public void OnRotated()
        {
            _orientationUpdated();
            
            if (OrientationChanged != null)
            {
                OrientationChanged(this, EventArgs.Empty);
            }
        }

        void _orientationUpdated()
        {
            if (UIDevice.CurrentDevice.Orientation == UIDeviceOrientation.LandscapeLeft ||
                UIDevice.CurrentDevice.Orientation == UIDeviceOrientation.LandscapeRight)
            {
                Orientation = XPageOrientation.Landscape;
            }
            if (UIDevice.CurrentDevice.Orientation == UIDeviceOrientation.Portrait ||
                UIDevice.CurrentDevice.Orientation == UIDeviceOrientation.PortraitUpsideDown)
            {
                Orientation = XPageOrientation.Portrait;
            }

            UpsideDown = UIDevice.CurrentDevice.Orientation == UIDeviceOrientation.LandscapeRight
                         || UIDevice.CurrentDevice.Orientation == UIDeviceOrientation.PortraitUpsideDown;
        }
    }
}
