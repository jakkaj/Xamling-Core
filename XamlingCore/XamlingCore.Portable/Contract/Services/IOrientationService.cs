using System;
using XamlingCore.Portable.Model.Orientation;

namespace XamlingCore.Portable.Contract.Services
{
    public interface IOrientationService
    {
        XPageOrientation CurrentPageOrientation { get; }
        bool IsUpsideDown { get; }
        XSupportedPageOrientation SupportedPageOrientation { get; }
        event EventHandler OrientationChanged;
        void SetSupportedOrientation(XSupportedPageOrientation supported);
        void SetCurrentOrientation(XPageOrientation orientation, bool isUpsideDown);
        event EventHandler SupportedOrientationChanged;
    }
}