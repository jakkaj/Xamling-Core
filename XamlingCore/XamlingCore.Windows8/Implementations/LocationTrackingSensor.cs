using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using XamlingCore.Portable.Contract.Device.Location;
using XamlingCore.Portable.Model.Location;
using XamlingCore.Portable.Util.GeoSpatial;

namespace XamlingCore.Windows8.Implementations
{
    public class LocationTrackingSensor : ILocationTrackingSensor
    {
        private Geolocator _locator;

        public LocationTrackingSensor()
        {
            _locator = new Geolocator();
            _locator.DesiredAccuracyInMeters = 15;
            _locator.DesiredAccuracy = PositionAccuracy.High;
        }

        public void Init()
        {
            
        }

        public void StartTracking()
        {
            throw new NotImplementedException();
        }

        public void StopTracking()
        {
            //throw new NotImplementedException();
        }

        public async Task<XLocation> GetQuickLocation()
        {
            try
            {
                var loc = await _locator.GetGeopositionAsync(TimeSpan.FromMinutes(5), TimeSpan.FromSeconds(8));

                if (loc == null)
                {
                    return null;
                }

                var xloc = new XLocation
                {
                    Accuracy = loc.Coordinate.Accuracy,
                    Altitude = loc.Coordinate.Altitude,
                    AltitudeAccuracy = loc.Coordinate.AltitudeAccuracy,
                    Heading = loc.Coordinate.Heading,
                    IsEnabled = true,
                    IsResolved = true,
                    Latitude = loc.Coordinate.Latitude,
                    Longitude = loc.Coordinate.Longitude,
                    Speed = loc.Coordinate.Speed,

                };

                CurrentLocation = xloc;

                return xloc;
            }
            catch { }

            return null;
        }

        public bool IsTracking { get; set; }
        public XLocation CurrentLocation { get; private set; }
        public event EventHandler LocationUpdated;

        public double Distance(double lat, double lng, XLocation b)
        {
            var xloca = new XLocation {Latitude = lat, Longitude = lng};
            return DistanceHelper.DistanceBetween(xloca, b, DistanceType.Kilometers);
        }

        public bool IsLocationEnabledInDeviceSettings()
        {
            return _locator.LocationStatus != PositionStatus.Disabled;
        }
    }
}
