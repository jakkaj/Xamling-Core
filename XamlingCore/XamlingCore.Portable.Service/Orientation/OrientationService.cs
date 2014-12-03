using System;
using XamlingCore.Portable.Contract.Device;
using XamlingCore.Portable.Contract.Services;
using XamlingCore.Portable.Messages.Device;
using XamlingCore.Portable.Messages.XamlingMessenger;
using XamlingCore.Portable.Model.Orientation;

namespace XamlingCore.Portable.Service.Orientation
{
    public class OrientationService : IOrientationService
    {
        private readonly IOrientationSensor _orientationSensor;

        public event EventHandler SupportedOrientationChanged;

        public OrientationService(IOrientationSensor orientationSensor)
        {
            SupportedPageOrientation = XSupportedPageOrientation.Both;
            _orientationSensor = orientationSensor;
            _orientationSensor.OrientationChanged += _orientationSensor_OrientationChanged;
        }

        void _orientationSensor_OrientationChanged(object sender, EventArgs e)
        {
            SetCurrentOrientation(_orientationSensor.Orientation, _orientationSensor.UpsideDown);
        }

        public event EventHandler OrientationChanged;

        protected virtual void OnOrientationChanged()
        {
            new PageOrientationChangedMessage(CurrentPageOrientation, IsUpsideDown).Send();
            var handler = OrientationChanged;
            if (handler != null) handler(this, EventArgs.Empty);
        }

        public void SetCurrentOrientation(XPageOrientation orientation, bool isUpsideDown)
        {
            if (CurrentPageOrientation == orientation)
            {
                return;
            }

            IsUpsideDown = isUpsideDown;
            CurrentPageOrientation = orientation;
            OnOrientationChanged();
        }

        public void SetSupportedOrientation(XSupportedPageOrientation supported)
        {
            SupportedPageOrientation = supported;
            new SupportedOrientationChangedMessage(supported).Send();
            if (SupportedOrientationChanged != null)
            {
                SupportedOrientationChanged(this, EventArgs.Empty);
            }
        }

        public XPageOrientation CurrentPageOrientation { get; protected set; }

        public bool IsUpsideDown { get; protected set; }

        public XSupportedPageOrientation SupportedPageOrientation { get; private set; }
    }
}
