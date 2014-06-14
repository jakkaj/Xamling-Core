using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using XamlingCore.Portable.Contract.Navigation;

namespace XamlingCore.iOS.Navigation
{
    public class iOSNavigator
    {
        private readonly IXNavigationService _xNavigationService;

        public iOSNavigator(IXNavigationService xNavigationService)
        {
            _xNavigationService = xNavigationService;

            _configure();
        }

        void _configure()
        {
            _xNavigationService.Navigated += _xNavigationService_Navigated;
        }

        void _xNavigationService_Navigated(object sender, EventArgs e)
        {
            
        }
    }
}