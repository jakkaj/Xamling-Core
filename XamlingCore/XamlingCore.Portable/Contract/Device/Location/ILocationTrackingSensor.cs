using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XamlingCore.Portable.Model.Location;

namespace XamlingCore.Portable.Contract.Device.Location
{
    public interface ILocationTrackingSensor
    {
        void StartTracking();
        void StopTracking();

        Task<XLocation> GetQuickLocation();

        bool IsTracking { get; set; }
        XLocation CurrentLocation { get; }

        event EventHandler LocationUpdated;

        double Distance(double lat, double lng, XLocation b);
        bool IsLocationEnabledInDeviceSettings();
    }
}
