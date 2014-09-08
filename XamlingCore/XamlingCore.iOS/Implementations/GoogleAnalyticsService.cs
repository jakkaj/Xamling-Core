//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using GoogleAnalytics.iOS;
//using MonoTouch.Foundation;
//using MonoTouch.UIKit;
//using XamlingCore.Portable.Contract.Services;

//namespace XamlingCore.iOS.Implementations
//{
//    public class GoogleAnalyticsService : IGoogleAnalyticsService
//    {
//        // Shared GA tracker
//        public IGAITracker Tracker;

//        // Learn how to get your own Tracking Id from:
//        // https://support.google.com/analytics/answer/2614741?hl=en
//        public static string TrackingId;
//        public static string AppName;
//        public static string AppVersion;

//        public GoogleAnalyticsService()
//        {
//            if (!string.IsNullOrWhiteSpace(TrackingId))
//            { 
//                Tracker = GAI.SharedInstance.GetTracker(TrackingId);

//                GAI.SharedInstance.DefaultTracker.Set(GAIConstants.AppName, AppName);
//                GAI.SharedInstance.DefaultTracker.Set(GAIConstants.AppVersion, AppVersion);
//            }
//        }

//        public double DispatchInterval
//        {
//            get { return GAI.SharedInstance.DispatchInterval; }
//            set { GAI.SharedInstance.DispatchInterval = value; }
//        }

//        public bool TrackUncaughtExceptions 
//        {
//            get { return GAI.SharedInstance.TrackUncaughtExceptions; }
//            set { GAI.SharedInstance.TrackUncaughtExceptions = value; } 
//        }

//        public bool OptOut
//        {
//            get { return GAI.SharedInstance.OptOut; }
//            set { GAI.SharedInstance.OptOut = value; }
//        }

//        public bool DryRun
//        {
//            get { return GAI.SharedInstance.DryRun; }
//            set { GAI.SharedInstance.DryRun = value; }
//        }

//        public void SendEvent(string category, string actionText, string label)
//        {
//            if (GAI.SharedInstance.DefaultTracker == null) return;
//            GAI.SharedInstance.DefaultTracker.Send(GAIDictionaryBuilder.CreateEvent(category, actionText, label, null).Build());
//        }

//        public void SendTiming(string category, int interval, string name, string label)
//        {
//            if (GAI.SharedInstance.DefaultTracker == null) return;
//            GAI.SharedInstance.DefaultTracker.Send(GAIDictionaryBuilder.CreateTiming(category, interval, name, label).Build());
//        }

//        public void SendException(string description, bool fatal)
//        {
//            if (GAI.SharedInstance.DefaultTracker == null) return;
//            GAI.SharedInstance.DefaultTracker.Send(GAIDictionaryBuilder.CreateException(description, fatal).Build());
//        }

//        public void SetView(string screenName)
//        {
//            if (GAI.SharedInstance.DefaultTracker == null) return;

//            GAI.SharedInstance.DefaultTracker.Set(GAIConstants.ScreenName, screenName);
//            GAI.SharedInstance.DefaultTracker.Send(GAIDictionaryBuilder.CreateAppView().Build());
//        }
//    }
//}