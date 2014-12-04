using Autofac;
using MonoTouch.UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using XamlingCore.iOS.Controls.Pages;
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

        public override bool ShouldAutomaticallyForwardRotationMethods
        {
            get { return true; }
        }

        public override UIInterfaceOrientationMask GetSupportedInterfaceOrientations()
        {
            switch (_orientationService.SupportedPageOrientation)
            {
                case XSupportedPageOrientation.Both:
                    return UIInterfaceOrientationMask.All;

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