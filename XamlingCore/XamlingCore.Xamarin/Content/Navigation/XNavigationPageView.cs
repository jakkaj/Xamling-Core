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

        private uint _animateTime = 250;
        private double _animateOffset = 300;
        private double _fadedOpacity = .3;

        private bool _animationIsEnabled;

        public event EventHandler BackButtonPressed;

        public XNavigationPageView(IOrientationSensor orientationSensor)
        {
            this.BackgroundColor = Color.Red;
            _orientationSensor = orientationSensor;
        }

        protected override bool OnBackButtonPressed()
        {
            if (BackButtonPressed != null)
            {
                BackButtonPressed(this, EventArgs.Empty);
                return true;
            }

            return base.OnBackButtonPressed();
        }

        bool _isAnimated()
        {
            return Device.OS == TargetPlatform.Windows;
        }

        public void PrepForAnimation()
        {
            if (!_isAnimated())
            {
                return;
            }

            _animationIsEnabled = true;

            this.TranslationY = _animateOffset;
            this.Opacity = _fadedOpacity;
        }

        public void AnimateIn()
        {
            this.FadeTo(1, _animateTime, Easing.CubicOut);
            this.TranslateTo(0, 0, _animateTime, Easing.CubicOut);
        }

        public async Task AnimateOut()
        {
            await Task.WhenAll(this.FadeTo(_fadedOpacity, _animateTime, Easing.CubicOut),
            this.TranslateTo(0, _animateOffset, _animateTime, Easing.CubicOut));
            
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            _orientationSensor.OnRotated();
            base.OnSizeAllocated(width, height);
        }

       
    }
}
