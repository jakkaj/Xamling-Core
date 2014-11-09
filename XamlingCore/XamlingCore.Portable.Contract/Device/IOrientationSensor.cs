using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XamlingCore.Portable.Model.Orientation;

namespace XamlingCore.Portable.Contract.Device
{
    public interface IOrientationSensor
    {
        event EventHandler OrientationChanged;
        XPageOrientation Orientation { get; }
        bool UpsideDown { get; }
        void OnRotated();
    }
}
