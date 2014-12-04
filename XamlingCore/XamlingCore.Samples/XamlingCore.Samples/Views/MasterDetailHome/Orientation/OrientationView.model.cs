using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using XamlingCore.Portable.Contract.Services;
using XamlingCore.Portable.Model.Orientation;
using XamlingCore.Portable.View.ViewModel;
using XamlingCore.Samples.Views.MasterDetailHome.Orientation.Temp;

namespace XamlingCore.Samples.Views.MasterDetailHome.Orientation
{
    public class OrientationViewModel : XViewModel
    {
        private readonly IOrientationService _orientationService;
        public ICommand PortraitCommand { get; set; }
        public ICommand LandscapeCommand { get; set; }
        public ICommand BothCommand { get; set; }

        public ICommand PopCommand { get; set; }

        public ICommand PushCommand { get; set; }

        public OrientationViewModel(IOrientationService orientationService)
        {
            Title = "Orientation";
            
            _orientationService = orientationService;

            PortraitCommand = new Command(_onPortrait);
            LandscapeCommand = new Command(_onLandscape);
            BothCommand = new Command(_onBoth);
            PopCommand = new Command(_onPop);
            PushCommand = new Command(_onPush);
        }
       
        void _onPush()
        {
            NavigateToModal(CreateContentModel<OrientationViewModel>());
        }

        void _onPop()
        {
            NavigateBack();
        }

        void _onBoth()
        {
            _orientationService.SetSupportedOrientation(XSupportedPageOrientation.Both);
        }

        void _onPortrait()
        {
            _orientationService.SetSupportedOrientation(XSupportedPageOrientation.Portrait);
        }

        void _onLandscape()
        {
            _orientationService.SetSupportedOrientation(XSupportedPageOrientation.Landscape);
        }

    }
}
