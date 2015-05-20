//using System.Threading.Tasks;
//using Autofac;
//using UIKit;
//using Xamarin.Forms;
//using Xamarin.Forms.Platform.iOS;
//using XamlingCore.Portable.Contract.Services;
//using XamlingCore.Portable.Data.Glue;
//using XamlingCore.Portable.Messages.XamlingMessenger;
//using XamlingCore.Portable.Model.Orientation;
//using XamlingCore.Samples.iOS.NativeViews.Renderer;
//using XamlingCore.XamarinThings.Content.Navigation;
//using XamlingCore.XamarinThings.Messages;

//[assembly: ExportRenderer(typeof(XNavigationPageView), typeof(iOSNavigationPageRenderer))]
//namespace XamlingCore.Samples.iOS.NativeViews.Renderer
//{
//    public class iOSNavigationPageRenderer : NavigationRenderer
//    {
//        private readonly IOrientationService _orientationService;

//        public iOSNavigationPageRenderer()
//        {
//            _orientationService = ContainerHost.Container.Resolve<IOrientationService>();
//        }

//        protected override Task<bool> OnPopViewAsync(Page page, bool animated)
//        {
//            var e = this.Element;


//            new PagePoppedMessage(page).Send();
//            return base.OnPopViewAsync(page, animated);
//        }

//        public override bool ShouldAutomaticallyForwardRotationMethods
//        {
//            get { return false; }
//        }

//        public override UIInterfaceOrientationMask GetSupportedInterfaceOrientations()
//        {

//            switch (_orientationService.SupportedPageOrientation)
//            {
//                case XSupportedPageOrientation.Both:
//                    return UIInterfaceOrientationMask.AllButUpsideDown;

//                case XSupportedPageOrientation.Landscape:
//                    return UIInterfaceOrientationMask.Landscape;

//                case XSupportedPageOrientation.Portrait:
//                    return UIInterfaceOrientationMask.Portrait;
//                default:
//                    return UIInterfaceOrientationMask.Portrait;
//            }
//        }
//    }
//}