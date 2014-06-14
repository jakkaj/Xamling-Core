using XamlingCore.Portable.DTO.Orientation;
using XamlingCore.Portable.XamlingMessenger;

namespace XamlingCore.Portable.Messages.Device
{
    public class PageOrientationChangedMessage : XMessage
    {
        private readonly XPageOrientation _orientation;
        private readonly bool _isUpsideDown;

        public PageOrientationChangedMessage(XPageOrientation orientation, bool isUpsideDown)
        {
            _orientation = orientation;
            _isUpsideDown = isUpsideDown;
        }

        public XPageOrientation Orientation
        {
            get { return _orientation; }
        }

        public bool IsUpsideDown
        {
            get { return _isUpsideDown; }
        }
    }
}
