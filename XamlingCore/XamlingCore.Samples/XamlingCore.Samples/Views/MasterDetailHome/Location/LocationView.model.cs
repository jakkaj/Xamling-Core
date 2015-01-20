using System;
using System.Diagnostics;
using System.Windows.Input;
using Xamarin.Forms;
using XamlingCore.Portable.Contract.Services;
using XamlingCore.Portable.Model.Location;
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

        private XLocation loc2 = new XLocation();

        public XLocation CurrentLocation
        {
            get { return loc2; }
            set
            {
                loc2 = value;
                OnPropertyChanged(); }
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
            CurrentLocation = await _locationService.GetQuickLocation();
        }



        void _locationService_LocationUpdated(object sender, EventArgs e)
        {
            CurrentLocation = _locationService.CurrentLocation;
        }


    }
}
