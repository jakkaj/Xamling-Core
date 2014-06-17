using System;
using XamlingCore.Portable.Contract.Services;
using XamlingCore.Portable.Messages.Device;
using XamlingCore.Portable.Messages.XamlingMessenger;
using XamlingCore.Portable.Model.Orientation;

namespace XamlingCore.Portable.Service.Orientation
{
    public class OrientationService : IOrientationService
    {
        public event EventHandler OrientationChanged;

        protected virtual void OnOrientationChanged()
        {
            new PageOrientationChangedMessage(CurrentPageOrientation, IsUpsideDown).Send();
            var handler = OrientationChanged;
            if (handler != null) handler(this, EventArgs.Empty);
        }

        public void SetOrientation(XPageOrientation orientation, bool isUpsideDown)
        {
            IsUpsideDown = isUpsideDown;
            CurrentPageOrientation = orientation;
            OnOrientationChanged();
        }

        public void SetSupportedOrientation(XSupportedPageOrientation supported)
        {
            new SupportedOrientationChangedMessage(supported).Send();
        }

        public XPageOrientation CurrentPageOrientation { get; protected set; }

        public bool IsUpsideDown { get; protected set; }
    }
}
