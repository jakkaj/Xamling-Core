using MonoTouch.Foundation;
using MonoTouch.UIKit;

using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using XamlingCore.iOS;
using XamlingCore.Samples.iOS.Glue;
using XamlingCore.Samples.Views.Home;
using XamlingCore.Samples.Views.Root.MasterDetailRoot;
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
        private XiOSCore<XRootFrame, RootMasterDetailViewModel, ProjectGlue> xCore;
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

            _configurePretties();

            //boot using standard navigation page 
            //var x = new XiOSCore<XRootFrame, XNavigationPageTypedViewModel<MainNavigationHomeViewModel>, ProjectGlue>();
            //x.Init();
            
            //boot using master detail setup
            xCore = new XiOSCore<XRootFrame, RootMasterDetailViewModel, ProjectGlue>();
            xCore.Init();

            return true;
        }

        void _configurePretties()
        {
            UINavigationBar.Appearance.BackgroundColor = UIColor.FromRGBA(0, 0, 0, 0);
            UINavigationBar.Appearance.TintColor = Color.Blue.ToUIColor();
            UINavigationBar.Appearance.BarTintColor = UIColor.White;
            UINavigationBar.Appearance.SetTitleTextAttributes(new UITextAttributes()
            {
                TextColor = UIColor.White
            });
        }
    }
}
