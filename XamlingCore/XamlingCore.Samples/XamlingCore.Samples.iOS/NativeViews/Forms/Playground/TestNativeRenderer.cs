using System;
using System.Diagnostics;
using System.Threading.Tasks;
using CoreGraphics;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using XamlingCore.Samples.iOS.NativeViews.Forms.Playground;
using XamlingCore.Samples.Views.MasterDetailHome.Home;

[assembly: ExportRenderer(typeof(HomeView), typeof(TestNativePageRenderer))]
namespace XamlingCore.Samples.iOS.NativeViews.Forms.Playground
{

    public class TestNativePageRenderer : PageRenderer
    {


        public override async void ViewDidLoad()
        {
            base.ViewDidLoad();
            _doLoad();
        }

        async void _doLoad()
        {
            await Task.Delay(2000);

            while (this.NavigationController == null)
            {
                await Task.Delay(1000);
                Debug.WriteLine("No nav");
            }
            this.NavigationController.Toolbar = UIBarPosition.Bottom;
            this.SetToolbarItems(new UIBarButtonItem[] {
                    new UIBarButtonItem(UIBarButtonSystemItem.Refresh, (s,e) => {
                        Console.WriteLine("Refresh clicked");
                    })
                    , new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace) { Width = 50 }
                    , new UIBarButtonItem(UIBarButtonSystemItem.Pause, (s,e) => {
                        Console.WriteLine ("Pause clicked");
                    })
                }, false);


            
            this.NavigationController.SetToolbarHidden(false, true);

        }

    }
}