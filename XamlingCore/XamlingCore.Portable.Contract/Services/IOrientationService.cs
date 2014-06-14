using System;
using XamlingCore.Portable.DTO.Orientation;

namespace XamlingCore.Portable.Contract.Services
{
    public interface IOrientationService
    {
        XPageOrientation CurrentPageOrientation { get; }
        bool IsUpsideDown { get; }
        event EventHandler OrientationChanged;
        void SetSupportedOrientation(XSupportedPageOrientation supported);
        void SetOrientation(XPageOrientation orientation, bool isUpsideDown);
    }
}