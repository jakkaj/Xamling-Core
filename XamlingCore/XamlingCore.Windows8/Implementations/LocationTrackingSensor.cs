using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XamlingCore.Portable.Contract.Device.Location;
using XamlingCore.Portable.Model.Location;

namespace XamlingCore.Windows8.Implementations
{
    public class LocationTrackingSensor : ILocationTrackingSensor
    {
        public void StartTracking()
        {
            throw new NotImplementedException();
        }

        public void StopTracking()
        {
            throw new NotImplementedException();
        }

        public Task<XLocation> GetQuickLocation()
        {
            throw new NotImplementedException();
        }

        public bool IsTracking { get; set; }
        public XLocation CurrentLocation { get; private set; }
        public event EventHandler LocationUpdated;
        public double Distance(double lat, double lng, XLocation b)
        {
            throw new NotImplementedException();
        }

        public bool IsLocationEnabledInDeviceSettings()
        {
            throw new NotImplementedException();
        }
    }
}
