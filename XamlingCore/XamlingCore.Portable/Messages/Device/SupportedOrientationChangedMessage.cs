using XamlingCore.Portable.Messages.XamlingMessenger;
using XamlingCore.Portable.Model.Orientation;

namespace XamlingCore.Portable.Messages.Device
{
    public class SupportedOrientationChangedMessage : XMessage
    {
        private readonly XSupportedPageOrientation _supportedOrientation;

        public SupportedOrientationChangedMessage(XSupportedPageOrientation supportedOrientation)
        {
            _supportedOrientation = supportedOrientation;
        }

        public XSupportedPageOrientation SupportedOrientation
        {
            get { return _supportedOrientation; }
        }
    }
}
