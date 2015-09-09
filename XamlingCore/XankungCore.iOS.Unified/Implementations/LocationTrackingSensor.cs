using System;
using System.Threading;
using System.Threading.Tasks;
using CoreLocation;
using Foundation;
using XamlingCore.iOS.Unified.Implementations.Location;
using XamlingCore.Portable.Contract.Device.Location;
using XamlingCore.Portable.Model.Location;

namespace XamlingCore.iOS.Unified.Implementations
{
    public class LocationTrackingSensor : ILocationTrackingSensor
    {
        private readonly TaskScheduler _scheduler = TaskScheduler.FromCurrentSynchronizationContext();
        private CancellationTokenSource _cancelSource;
        private Geolocator _geolocator;

        public LocationTrackingSensor()
        {
            Init();
        }

        public event EventHandler LocationUpdated;

        public void StartTracking()
        {
            Setup();

            CurrentLocation.IsEnabled = true;

            if (IsTracking) return;

            if (!_geolocator.IsListening)
                _geolocator.StartListening(5000, 1, true);

            IsTracking = true;
            _fire();
        }


        public void StopTracking()
        {
            if (!IsTracking) return;
            CurrentLocation.IsEnabled = false;
            CurrentLocation.IsResolved = false;
            _geolocator.PositionError -= OnListeningError;
            _geolocator.PositionChanged -= OnPositionChanged;
            _geolocator.StopListening();

            IsTracking = false;

            _geolocator = null;
        }

        public async Task<XLocation> GetQuickLocation()
        {
            Setup();

            var loc = new XLocation();

            _cancelSource = new CancellationTokenSource();

            await _geolocator.GetPositionAsync(1500, _cancelSource.Token, false)
                .ContinueWith(t =>
                {

                    if (t.IsCanceled || t.IsFaulted)
                    {
                        var x = new XLocation();
                        x.IsResolved = false;
                        x.Status = XPositionStatus.NotAvailble;
                        return x;
                    }
                        
                    loc.Latitude = t.Result.Latitude;
                    loc.Longitude = t.Result.Longitude;
                    loc.Accuracy = t.Result.Accuracy;
                    loc.Heading = t.Result.Heading;
                    loc.IsEnabled = true;
                    loc.IsResolved = true;
                    loc.Status = XPositionStatus.Ready;
                    return loc;
                }, _scheduler);
            return loc;
        }


        public bool IsTracking { get; set; }
        public XLocation CurrentLocation { get; private set; }

        public double Distance(double lat, double lng, XLocation b)
        {
            var firstPoint = new CLLocation(lat, lng);
            var secondPoint = new CLLocation(b.Latitude, b.Longitude);

            return firstPoint.DistanceFrom(secondPoint);
        }

        public bool IsLocationEnabledInDeviceSettings()
        {   
            return CLLocationManager.LocationServicesEnabled;
        }

        private void Init()
        {
            CurrentLocation = new XLocation();
            
        }

        private void Setup()
        {
            if (_geolocator != null)
                return;

            _geolocator = new Geolocator {DesiredAccuracy = 10};
            _geolocator.PositionError += OnListeningError;
            _geolocator.PositionChanged += OnPositionChanged;
        }


        private void CancelPosition(NSObject sender)
        {
            CancellationTokenSource cancel = _cancelSource;
            if (cancel != null)
                cancel.Cancel();
        }


        private void OnListeningError(object sender, PositionErrorEventArgs e)
        {
            switch (e.Error)
            {
                case GeolocationError.PositionUnavailable:
                    CurrentLocation.IsResolved = false;
                    CurrentLocation.Status = XPositionStatus.NotAvailble;
                    _fire();
                    break;
                case GeolocationError.Unauthorized:
                    CurrentLocation.IsResolved = false;
                    CurrentLocation.IsEnabled = false;
                    CurrentLocation.Status = XPositionStatus.Disabled;
                    _fire();
                    break;
                default:
                     CurrentLocation.IsResolved = false;
                   CurrentLocation.Status = XPositionStatus.NotInitialized;
                    _fire();
                    break;
            }
        }

        private void OnPositionChanged(object sender, PositionEventArgs e)
        {
            _sendUpdate(e.Position);
        }

        private void _fire()
        {
            if (LocationUpdated != null)
            {
                LocationUpdated(this, EventArgs.Empty);
            }
        }

        private void _sendUpdate(Position location)
        {
            if (location == null)
                CurrentLocation.IsResolved = false;
            else
            {
                CurrentLocation.Accuracy = location.Accuracy;
                CurrentLocation.Latitude = location.Latitude;
                CurrentLocation.Longitude = location.Longitude;
                CurrentLocation.Heading = location.Heading;
                CurrentLocation.HeadingAvailable = _geolocator.SupportsHeading;
                CurrentLocation.Speed = location.Speed;
                CurrentLocation.Altitude = location.Altitude;
                CurrentLocation.AltitudeAccuracy = location.AltitudeAccuracy;
                // assume that any location event that came through is valid
                // and also assume this property means sorrthe location is legit
                CurrentLocation.IsResolved = true;
                CurrentLocation.Status = XPositionStatus.Ready;
            }

            _fire();
        }
    }
}