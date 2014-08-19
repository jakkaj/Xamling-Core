using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoTouch.CoreLocation;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using XamlingCore.Portable.Contract.Device.Location;
using XamlingCore.Portable.Model.Location;

namespace XamlingCore.iOS.Implementations
{
    public class LocationTrackingSensor : ILocationTrackingSensor
    {
        public event EventHandler LocationUpdated;

        private CLLocationManager _geolocator;

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

            _geolocator = new CLLocationManager {DesiredAccuracy = CLLocation.AccuracyBest, DistanceFilter = 1};

		    _geolocator.LocationsUpdated += GeolocatorOnLocationsUpdated;
            
            IsTracking = true;
            _fire();
        }

        private void GeolocatorOnLocationsUpdated(object sender, CLLocationsUpdatedEventArgs args)
        {
            foreach (var clLocation in args.Locations)
            {
                _sendUpdate(clLocation);
            }
        }

        public void StopTracking()
        {
            if (!IsTracking) return;
            CurrentLocation.IsEnabled = false;
            CurrentLocation.IsResolved = false;
            _geolocator.LocationsUpdated -= GeolocatorOnLocationsUpdated;
            _geolocator.StopUpdatingLocation();

            IsTracking = false;

            _geolocator.Dispose();
            _geolocator = null;

        }

        public Task<XLocation> GetQuickLocation()
        {
            // need to work out the implementation for this 
            // i think i need to start the listener but only
            // listen to one event and then stop.
            throw new NotImplementedException();
        }

        public bool IsTracking { get; set; }
        public XLocation CurrentLocation { get; private set; }
        public double Distance(double lat, double lng, XLocation b)
        {

            CLLocation firstPoint = new CLLocation(lat, lng);
            var secondPoint = new CLLocation(b.Latitude, b.Longitude);

            return firstPoint.DistanceFrom(secondPoint);
        }

        public bool IsLocationEnabledInDeviceSettings()
        {
            return CLLocationManager.LocationServicesEnabled;
        }

        void _fire()
        {
            if (LocationUpdated != null)
            {
                LocationUpdated(this, EventArgs.Empty);
            }
        }
        
        void _sendUpdate(CLLocation location)
        {
            CurrentLocation.Accuracy = location.HorizontalAccuracy;
            CurrentLocation.Latitude = location.Coordinate.Latitude;
            CurrentLocation.Longitude = location.Coordinate.Longitude;

            // assume that any location event that came through is valid
            // and also assume this property means the location is legit
            CurrentLocation.IsResolved = true;

            _fire();

        }

    }
}