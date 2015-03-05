using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;
using XamlingCore.Portable.Contract.Device;

namespace XamlingCore.XamarinThings.Content.Navigation
{
    public class XNavigationPageView : NavigationPage
    {
        private readonly IOrientationSensor _orientationSensor;

        public XNavigationPageView(IOrientationSensor orientationSensor)
        {
            _orientationSensor = orientationSensor;
            
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            _orientationSensor.OnRotated();
            base.OnSizeAllocated(width, height);
        }

       
    }
}
