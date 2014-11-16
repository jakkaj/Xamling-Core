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

        public RootViewController(IOrientationService orientationService)
        {
            _orientationService = orientationService;
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
