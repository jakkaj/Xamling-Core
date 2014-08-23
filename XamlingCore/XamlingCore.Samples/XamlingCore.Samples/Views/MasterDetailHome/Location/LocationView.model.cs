using System;
using System.Diagnostics;
using System.Windows.Input;
using Xamarin.Forms;
using XamlingCore.Portable.Contract.Services;
using XamlingCore.Portable.View.ViewModel;

namespace XamlingCore.Samples.Views.MasterDetailHome.Location
{
    public class LocationViewModel : XViewModel
    {
        private ILocationService _locationService;
        public ICommand StartLocationCommand { get; set; }
        public ICommand StopLocationCommand { get; set; }
        public ICommand GetLocationCommand { get; set; }

        public LocationViewModel(ILocationService locationService)
        {
            Title = "Location";
            StartLocationCommand = new Command(_startLocation);
            StopLocationCommand = new Command(_stopLocation);
            GetLocationCommand = new Command(_getLocation);

            // wire up all that location goodness
            _locationService = locationService;
            _locationService.LocationUpdated += _locationService_LocationUpdated;
        }

        void _startLocation()
        {
            Debug.WriteLine("Start Location Command");
            _locationService.Start();
        }

        void _stopLocation()
        {
            Debug.WriteLine("Stop Location Command");
            _locationService.Stop();
        }

        async void _getLocation()
        {
            var loc = await _locationService.GetQuickLocation();
            Debug.WriteLine("lat: {0}, long: {1}, Accuracy: {2}, Status: {3}, Enabled {4}, Resolved {5}", loc.Latitude, loc.Longitude, loc.Accuracy, loc.Status, loc.IsEnabled, loc.IsResolved);

        }

        void _locationService_LocationUpdated(object sender, EventArgs e)
        {
            //todo: need to bind this to form display
            Debug.WriteLine("Location Updated");
            var loc = _locationService.CurrentLocation;
            if (loc == null)
            {
                Debug.WriteLine("No Location Data");
            }
            else
            { 
                Debug.WriteLine("lat: {0}, long: {1}, Accuracy: {2}, Status: {3}, Enabled {4}, Resolved {5}", loc.Latitude, loc.Longitude, loc.Accuracy, loc.Status, loc.IsEnabled, loc.IsResolved);
            }
        }

    }
}
