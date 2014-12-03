using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using System.Text;
using MonoTouch.UIKit;
using XamlingCore.Portable.Contract.Services;
using XamlingCore.Portable.Model.Orientation;

namespace XamlingCore.iOS.Root
{
    public class RootViewController : UIViewController
    {
        private readonly IOrientationService _orientationService;
        private UIViewController _controller;
        private UIWindow _window;
        public RootViewController(IOrientationService orientationService)
        {
            _orientationService = orientationService;

            _orientationService.OrientationChanged += _orientationService_OrientationChanged;
            _orientationService.SupportedOrientationChanged += _orientationService_SupportedOrientationChanged;
        }

        public void SetChild(UIViewController controller, UIWindow window)
        {
            _controller = controller;
            _window = window;

            AddChildViewController(_controller);
            View.AddSubview(_controller.View);
        }

        void _refreshChild()
        {
            _window.RootViewController = new UIViewController();

            _window.RootViewController = this;
        }

        void _orientationService_SupportedOrientationChanged(object sender, EventArgs e)
        {
            _refreshChild();
        }

        void _orientationService_OrientationChanged(object sender, EventArgs e)
        {
           
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
