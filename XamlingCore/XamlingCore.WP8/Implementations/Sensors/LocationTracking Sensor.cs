using System;
using System.Device.Location;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using XamlingCore.Portable.Contract.Device.Location;
using XamlingCore.Portable.Model.Location;
using XamlingCore.WP8.Helpers;

namespace XamlingCore.WP8.Implementations.Sensors
{
    public class LocationTrackingSensor : ILocationTrackingSensor
    {
        private Geolocator _geolocator;

        public event EventHandler LocationUpdated;

        public LocationTrackingSensor()
        {
            init();
        }

        void init()
        {
            CurrentLocation = new XLocation();
        }

        public void StartTracking()
        {
            CurrentLocation.IsEnabled = true;

            if (IsTracking) return;

            _geolocator = new Geolocator()
            {
                DesiredAccuracy = PositionAccuracy.High,
                MovementThreshold = 1
            };

            _geolocator.StatusChanged += geolocator_StatusChanged;
            _geolocator.PositionChanged += geolocator_PositionChanged;

            IsTracking = true;
            _fire();
        }

        public void StopTracking()
        {
            if (!IsTracking) return;
            CurrentLocation.IsEnabled = false;
            CurrentLocation.IsResolved = false;
            _geolocator.StatusChanged -= geolocator_StatusChanged;
            _geolocator.PositionChanged -= geolocator_PositionChanged;
            _geolocator = null;

            IsTracking = false;
        }

        public async Task<XLocation> GetQuickLocation()
        {
            var geolocator = new Geolocator();

            geolocator.DesiredAccuracyInMeters = 1000;

            try
            {
                var geoposition = await geolocator.GetGeopositionAsync(
                     maximumAge: TimeSpan.FromMinutes(5),
                     timeout: TimeSpan.FromSeconds(20)
                     );

                return new XLocation()
                {
                    Accuracy = geoposition.Coordinate.Accuracy,
                    IsEnabled = true,
                    IsResolved = true,
                    Latitude = geoposition.Coordinate.Latitude,
                    Longitude = geoposition.Coordinate.Longitude,
                    Status = XPositionStatus.Ready
                };
            }
            catch (Exception ex)
            {
                if ((uint)ex.HResult == 0x80004004)
                {
                    return new XLocation
                    {

                        IsEnabled = false,
                        IsResolved = false,
                        Status = XPositionStatus.Disabled
                    };
                }

                return new XLocation
                {
                    IsEnabled = false,
                    IsResolved = false,
                    Status = XPositionStatus.NotAvailble
                };
            }
        }

        public bool IsLocationEnabledInDeviceSettings()
        {
            var watcher = new GeoCoordinateWatcher();
            return watcher.Permission != GeoPositionPermission.Denied;
        }

        void _sendUpdate(Geocoordinate currentPosition = null, double accuracy = 0)
        {
            if (currentPosition != null)
            {
                CurrentLocation = LocationHelpers.ConvertXLocation(currentPosition, CurrentLocation);
                CurrentLocation.IsResolved = true;
            }
            else
            {
                CurrentLocation.IsResolved = false;
            }

            if (accuracy != 0)
            {
                CurrentLocation.Accuracy = Convert.ToInt32(accuracy);
            }


            _fire();

        }

        void _fire()
        {
            if (LocationUpdated != null)
            {
                LocationUpdated(this, EventArgs.Empty);
            }
        }

        private void geolocator_StatusChanged(Geolocator sender, StatusChangedEventArgs args)
        {
            CurrentLocation.Status = LocationHelpers.ConvertLocationStatus(args.Status);
            _fire();
        }

        void geolocator_PositionChanged(Geolocator sender, PositionChangedEventArgs args)
        {
            var currentPosition = args.Position.Coordinate;
            var accuracy = args.Position.Coordinate.Accuracy;

            _sendUpdate(currentPosition, accuracy);
        }

        public double Distance(double lat, double lng, XLocation b)
        {
            return LocationHelpers.Distance(lat, lng, b);
        }

        public bool IsTracking { get; set; }

        public XLocation CurrentLocation { get; private set; }


    }
}
