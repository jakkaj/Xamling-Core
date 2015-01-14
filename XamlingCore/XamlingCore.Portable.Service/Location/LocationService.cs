using System;
using System.Threading.Tasks;
using AutoMapper;
using XamlingCore.Portable.Contract.Device.Location;
using XamlingCore.Portable.Contract.Services;
using XamlingCore.Portable.Messages.Location;
using XamlingCore.Portable.Messages.XamlingMessenger;
using XamlingCore.Portable.Model.Location;
using XamlingCore.Portable.Util;
using XamlingCore.Portable.Util.Lock;
using XamlingCore.Portable.Util.Util;

namespace XamlingCore.Portable.Service.Location
{
    public class LocationService : ILocationService
    {
        private readonly ILocationTrackingSensor _locationSensor;
        private readonly IGeneralSettingsService _generalSettingsService;

        public event EventHandler LocationUpdated;

        AsyncLock _lock = new AsyncLock();

        public LocationService(ILocationTrackingSensor locationSensor, IGeneralSettingsService generalSettingsService)
        {
            _locationSensor = locationSensor;
            _generalSettingsService = generalSettingsService;
            _locationSensor.LocationUpdated += _locationSensor_LocationUpdated;

            this.Register<LocationSettingsChangedMessage>(_onLocationSettingsChanged);
        }

        public async void Start()
        {
            if (await IsLocationEnabled())
            {
                _locationSensor.StartTracking();
            }
            else
            {
                _locationSensor.StopTracking();
            }

            _fireEvent();
        }

        public void Stop()
        {
            _locationSensor.StopTracking();
            _fireEvent();
        }

        public void _onLocationSettingsChanged(object o = null)
        {
            Start();
        }

        public bool IsLocationEnabledInDeviceSettings()
        {
            return _locationSensor.IsLocationEnabledInDeviceSettings();
        }

        public async Task<bool> IsLocationEnabled()
        {
            if (!_locationSensor.IsLocationEnabledInDeviceSettings())
            {
                return false;
            }

            var s = await _generalSettingsService.GetLocationEnabled();
            return s;
        }

        public bool IsLocationResolved()
        {
            if (CurrentLocation == null)
            {
                return false;
            }

            if (!CurrentLocation.IsEnabled || !CurrentLocation.IsResolved)
            {
                return false;
            }

            if (CurrentLocation.Status != XPositionStatus.Ready)
            {
                return false;
            }

            if (_absCheck(CurrentLocation.Latitude) || _absCheck(CurrentLocation.Longitude))
            {
                return false;
            }

            return true;
        }

        bool _absCheck(double dbl)
        {
            return (Math.Abs(dbl) <= 0.000000001 && Math.Abs(dbl) >= -0.000000001);
        } 

        public XLocation GetCurrentLocation()
        {
            return _locationSensor.CurrentLocation;
        }

        public async Task<XLocation> GetQuickLocation()
        {
            if (await IsLocationEnabled())
            {
                if (_locationSensor.CurrentLocation != null && _locationSensor.CurrentLocation.IsResolved)
                {
                    return _locationSensor.CurrentLocation;
                }

                return await _locationSensor.GetQuickLocation();
            }

            return new XLocation { IsEnabled = false };
        }

        async void _locationSensor_LocationUpdated(object sender, System.EventArgs e)
        {
            using (var l = await _lock.LockAsync())
            {
                var wasResolved = IsLocationResolved();

                //todo don't have AutoMapper configured - come back to this when configured
                //CurrentLocation = Mapper.Map(_locationSensor.CurrentLocation, loc); //clone it

                var loc = new XLocation
                {
                    Accuracy = _locationSensor.CurrentLocation.Accuracy,
                    IsEnabled = _locationSensor.CurrentLocation.IsEnabled,
                    IsResolved = _locationSensor.CurrentLocation.IsResolved,
                    Latitude = _locationSensor.CurrentLocation.Latitude,
                    Longitude = _locationSensor.CurrentLocation.Longitude,
                    Status = _locationSensor.CurrentLocation.Status,
                    Altitude = _locationSensor.CurrentLocation.Altitude,
                    AltitudeAccuracy = _locationSensor.CurrentLocation.AltitudeAccuracy,
                    Heading = _locationSensor.CurrentLocation.Heading,
                    HeadingAvailable =  _locationSensor.CurrentLocation.HeadingAvailable,
                    Speed = _locationSensor.CurrentLocation.Speed
                };

                CurrentLocation = loc;

                var isResolved = IsLocationResolved();

                if (!wasResolved && isResolved)
                {
                    LowResLocation = new XLocation
                    {
                        Latitude = CurrentLocation.Latitude,
                        Accuracy = CurrentLocation.Accuracy,
                        IsEnabled = CurrentLocation.IsEnabled,
                        IsResolved = CurrentLocation.IsResolved,
                        Longitude = CurrentLocation.Longitude,
                        Status = CurrentLocation.Status
                    };

                    new LowResLocationUpdatedMessage().Send();
                }
                else if (LowResLocation != null && _locationSensor.Distance(LowResLocation.Latitude, LowResLocation.Longitude, CurrentLocation) > 50)
                {
                    //todo don't have AutoMapper configured - come back to this when configured
                    //LowResLocation = Mapper.Map<XLocation>(CurrentLocation);

                    LowResLocation = new XLocation
                    {
                        Latitude = CurrentLocation.Latitude,
                        Accuracy = CurrentLocation.Accuracy,
                        IsEnabled = CurrentLocation.IsEnabled,
                        IsResolved = CurrentLocation.IsResolved,
                        Longitude = CurrentLocation.Longitude,
                        Status = CurrentLocation.Status
                    };
                    new LowResLocationUpdatedMessage().Send();
                }

                _fireEvent();
            }
          
        }

        void _fireEvent()
        {
            if (LocationUpdated != null)
            {
                LocationUpdated(this, EventArgs.Empty);
            }
        }

        public async Task<double?> Distance(double lat, double lng)
        {
            if (CurrentLocation == null)
            {
                return null;
            }

            if (CurrentLocation != null)
            {
                return _locationSensor.Distance(lat, lng, CurrentLocation);
            }

            return null;
        }

        public XLocation CurrentLocation { get; private set; }

        public XLocation LowResLocation { get; private set; }
    }
}
