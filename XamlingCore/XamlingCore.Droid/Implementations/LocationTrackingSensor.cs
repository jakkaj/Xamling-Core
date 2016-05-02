using Android.App;
using Android.Locations;
using Android.OS;
using Android.Runtime;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using XamlingCore.Portable.Contract.Device.Location;
using XamlingCore.Portable.Model.Location;

namespace XamlingCore.Droid.Implementations
{
    public class LocationTrackingSensor : ILocationTrackingSensor
    {
        //http://developer.xamarin.com/recipes/android/os_device_resources/gps/get_current_device_location/

        //ILocationListener has event binds etc already in place for 'OnLocationChanged' etc.

        public bool IsTracking { get; set; }
        public XLocation CurrentLocation { get; private set; }
        public event EventHandler LocationUpdated;

        private Location _currentLocation;
        private LocationManager _locationManager;
        private string _locationProvider;

        private JavaSensorHandler _handler;

        public LocationTrackingSensor()
        {
            _handler = new JavaSensorHandler(this);
            Init();
        }

        public class JavaSensorHandler : Java.Lang.Object, ILocationListener
        {
            private readonly LocationTrackingSensor _parent;

            public JavaSensorHandler(LocationTrackingSensor parent)
            {
                _parent = parent;
            }

            public void OnLocationChanged(Location location)
            {
                _parent.OnLocationChanged(location);
            }

            public void OnProviderDisabled(string provider)
            {
                _parent.OnProviderDisabled(provider);
            }

            public void OnProviderEnabled(string provider)
            {
                _parent.OnProviderEnabled(provider);
            }

            public void OnStatusChanged(string provider, Availability status, Bundle extras)
            {
                _parent.OnStatusChanged(provider, status, extras);
            }
        }


        public void Init()
        {
            CurrentLocation = new XLocation();
            Setup();
        }

        public void OnLocationChanged(Location location)
        {
            _currentLocation = location;
            _sendUpdate();
        }

        public void OnProviderDisabled(string provider)
        {
            _isUnavailable();
        }

        public void OnProviderEnabled(string provider) { /* Nothing? */ }

        public void OnStatusChanged(string provider, Availability status, Bundle extras)
        {
            switch (status)
            {
                case Availability.OutOfService:
                    _isUnavailable();
                    break;
                case Availability.TemporarilyUnavailable:
                    _isUnavailable();
                    break;
                case Availability.Available:
                    _isAvailable();
                    break;
            }
        }


        private void _isAvailable()
        {
            _fire();
        }

        private void _isUnavailable()
        {
            CurrentLocation.IsResolved = false;
            CurrentLocation.Status = XPositionStatus.NotAvailble;
            _fire();
        }

        private void Setup()
        {
            if (_locationManager == null)
            {
                _locationManager = (LocationManager)Application.Context.GetSystemService(Android.App.Activity.LocationService);
            }

            if (String.IsNullOrWhiteSpace(_locationProvider))
            {
                Criteria criteriaForLocationService = new Criteria
                {
                    Accuracy = Accuracy.Fine
                };

                _locationProvider = _locationManager.GetBestProvider(criteriaForLocationService, true);
            }
        }

        public void StartTracking()
        {
            Init();
            if (_locationProvider == null)
            {
                return;
            }

            CurrentLocation.IsEnabled = true;

            if (IsTracking) return;

            try
            {
                _locationManager.RequestLocationUpdates(_locationProvider, 2000, 1, _handler); //TODO: Set back to 5000,1,this
            }
            catch (Exception e)
            {
                return;
            }

            IsTracking = true;
            _fire();
        }


        public void StopTracking()
        {
            Init();
            if (_locationProvider == null)
            {
                return;
            }
            if (!IsTracking) return;

            CurrentLocation.IsEnabled = false;
            CurrentLocation.IsResolved = false;

            _locationManager.RemoveUpdates(_handler);

            IsTracking = false;

            _locationManager = null;
        }

        private void CancelPosition()
        {
            /* No action, GetQuickLocation not asyncronous */
        }

        public async Task<XLocation> GetQuickLocation()
        {
            Init();

            if (!string.IsNullOrWhiteSpace(_locationProvider) && _locationManager != null)
            {
                var l = _locationManager.GetLastKnownLocation(_locationProvider);

                if (l != null)
                {
                    var loc = new XLocation();

                    loc.Latitude = l.Latitude;
                    loc.Longitude = l.Longitude;
                    loc.Accuracy = l.Accuracy;
                    loc.Heading = l.Bearing;
                    loc.IsEnabled = true;
                    loc.IsResolved = true;
                    loc.Status = XPositionStatus.Ready;

                    return loc;
                }
            }

            return new XLocation();
        }



        public double Distance(double lat, double lng, XLocation b)
        {
            //Manual distance calculation.

            var R = 6371; // Radius of the earth in km
            var lat1 = lat;
            var lon1 = lng;
            var lat2 = b.Latitude;
            var lon2 = b.Longitude;

            var dLat = deg2rad(lat2 - lat1);
            var dLon = deg2rad(lon2 - lon1);
            var a =
              Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
              Math.Cos(deg2rad(lat1)) * Math.Cos(deg2rad(lat2)) *
              Math.Sin(dLon / 2) * Math.Sin(dLon / 2)
              ;
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            var d = R * c; // Distance in km
            return d;
        }

        private double deg2rad(double deg)
        {
            return deg * (Math.PI / 180);
        }

        public bool IsLocationEnabledInDeviceSettings()
        {
            //Note: This is only checking the currently selected provider?
            if (_locationManager == null || string.IsNullOrWhiteSpace(_locationProvider))
                return false;

            return _locationManager.IsProviderEnabled(_locationProvider);
        }

        private void _fire()
        {
            if (LocationUpdated != null)
            {
                LocationUpdated(this, EventArgs.Empty);
            }
        }

        private void _sendUpdate()
        {
            if (_currentLocation == null)
                CurrentLocation.IsResolved = false;
            else
            {
                CurrentLocation.Accuracy = _currentLocation.Accuracy;
                CurrentLocation.Latitude = _currentLocation.Latitude;
                CurrentLocation.Longitude = _currentLocation.Longitude;
                CurrentLocation.Heading = _currentLocation.Bearing;
                CurrentLocation.HeadingAvailable = _currentLocation.HasBearing;
                CurrentLocation.Speed = _currentLocation.Speed;
                CurrentLocation.Altitude = _currentLocation.Altitude;
                //CurrentLocation.AltitudeAccuracy = _currentLocation.AltitudeAccuracy; //Not supported
                CurrentLocation.IsResolved = true;
                CurrentLocation.Status = XPositionStatus.Ready;
            }

            _fire();
        }
    }
}