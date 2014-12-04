using System.Diagnostics;
using System.Threading.Tasks;
using Autofac;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using XamlingCore.iOS.Controls.Pages;
using XamlingCore.iOS.Root;
using XamlingCore.Portable.Contract.Services;
using XamlingCore.Portable.Data.Glue;
using XamlingCore.Portable.Model.Orientation;
using XamlingCore.XamarinThings.Content.Navigation;
using XamlingCore.XamarinThings.Content.Pages;

[assembly: ExportRenderer(typeof(XNavigationPageView), typeof(iOSNavigationPageRenderer))]
namespace XamlingCore.iOS.Controls.Pages
{
    public class iOSNavigationPageRenderer : NavigationRenderer
    {
        private readonly IOrientationService _orientationService;

        public iOSNavigationPageRenderer()
        {
            _orientationService = ContainerHost.Container.Resolve<IOrientationService>();
        }

        public override Task PresentViewControllerAsync(UIViewController viewControllerToPresent, bool animated)
        {
            return base.PresentViewControllerAsync(viewControllerToPresent, animated);
        }

        public override void PresentViewController(UIViewController viewControllerToPresent, bool animated, NSAction completionHandler)
        {
            base.PresentViewController(viewControllerToPresent, animated, completionHandler);
        }

        public override void PushViewController(UIViewController viewController, bool animated)
        {
            //var view = new RootViewController(_orientationService);

       
            //view.View.Add(viewController.View);
            //foreach (var item in viewController.View.Subviews)
            //{
            //    item.RemoveFromSuperview();
            //    view.View.AddSubview(item);
            //}

            ////viewController.RemoveFromParentViewController();

            ////view.AddChildViewController(viewController);

            //base.PushViewController(view, animated);
            ////_quickToggle();
           
            
            base.PushViewController(viewController, animated);
        }

        protected override Task<bool> OnPushAsync(Page page, bool animated)
        {
            //var view = new RootViewController(_orientationService);
            //var pView = page.CreateViewController();

            //view.AddChildViewController(pView);
            //view.View.Add(pView.View);

            return base.OnPushAsync(page, animated);
        }


        public override bool ShouldAutomaticallyForwardRotationMethods
        {
            get { return false; }
        }

        public override UIInterfaceOrientationMask GetSupportedInterfaceOrientations()
        {
            Debug.WriteLine("Navigation Orientation: {0}", _orientationService.SupportedPageOrientation);
            switch (_orientationService.SupportedPageOrientation)
            {
                case XSupportedPageOrientation.Both:
                    return UIInterfaceOrientationMask.AllButUpsideDown;

                case XSupportedPageOrientation.Landscape:
                    return UIInterfaceOrientationMask.Landscape;

                case XSupportedPageOrientation.Portrait:
                    return UIInterfaceOrientationMask.Portrait;
                default:
                    return UIInterfaceOrientationMask.Portrait;
            }
        }
    }
}