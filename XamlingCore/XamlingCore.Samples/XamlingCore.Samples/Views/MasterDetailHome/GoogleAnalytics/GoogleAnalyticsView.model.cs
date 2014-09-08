//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Input;
//using Xamarin.Forms;
//using XamlingCore.Portable.Contract.Services;
//using XamlingCore.Portable.Messages.View;
//using XamlingCore.Portable.View.ViewModel;

//namespace XamlingCore.Samples.Views.MasterDetailHome.GoogleAnalytics
//{
//    public class GoogleAnalyticsViewModel  : XViewModel
//    {
//        private readonly IGoogleAnalyticsService _googleAnalytics;
//        public ICommand SendEventCommand { get; set; }
//        public ICommand SendScreenCommand { get; set; }
//        public ICommand SendTimingCommand { get; set; }
//        public ICommand SendExceptionCommand { get; set; }

//        public GoogleAnalyticsViewModel(IGoogleAnalyticsService googleAnalytics)
//        {
//            Title = "Google Analytics";
//            _googleAnalytics = googleAnalytics;
//            _googleAnalytics.TrackUncaughtExceptions = true;
//            _googleAnalytics.DispatchInterval = 5;

//            SendEventCommand = new Command(_onSendEvent);
//            SendScreenCommand = new Command(_onSendScreen);
//            SendTimingCommand = new Command(_onSendTiming);
//            SendExceptionCommand = new Command(_onSendException);
//        }

//        public override void OnActivated()
//        {
//            base.OnActivated();
            
//            // this is probably how you will use Google Analytics most
//            // to register that we have activated a particular screen
//            _googleAnalytics.SetView(this.GetType().Name);
//        }

//        void _onSendEvent()
//        {
//            _googleAnalytics.SendEvent("UX", "Click", "OnSendEvent Button");
//        }

//        void _onSendScreen()
//        {
//            _googleAnalytics.SetView("GoogleAnalyticsView");
//        }

//        void _onSendTiming()
//        {
//            // apparently these values do not make it to the dashboard for day or two
//            _googleAnalytics.SendTiming("Performance", 69, "Some Name", "Some Label");
//        }

//        void _onSendException()
//        {
//            // apparently these values do not make it to the dashboard for day or two
//            _googleAnalytics.SendException("Oh Snap!", false);
//        }
//    }
//}
