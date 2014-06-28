using MonoTouch.Foundation;
using MonoTouch.UIKit;

using Xamarin.Forms;
using XamlingCore.iOS;
using XamlingCore.Samples.iOS.Glue;
using XamlingCore.Samples.View.Home;
using XamlingCore.Samples.View.Root.MasterDetailRoot;
using XamlingCore.XamarinThings.Content.Navigation;
using XamlingCore.XamarinThings.Frame;

namespace XamlingCore.Samples.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : UIApplicationDelegate
    {
        // class-level declarations
        UIWindow window;

        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            Forms.Init();

            //boot using standard navigation page 
            var x = new XiOSCore<XRootFrame, XNavigationPageTypedViewModel<MainNavigationHomeViewModel>, ProjectGlue>();
            x.Init();
            
            //boot using master detail setup
            //var x = new XiOSCore<XRootFrame, RootMasterDetailViewModel, ProjectGlue>();
            //x.Init();

            return true;
        }
    }
}
