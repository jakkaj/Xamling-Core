using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Xamarin.Forms;
using XamlingCore.Portable.View.ViewModel;
using XamlingCore.Xamarin.Contract;

namespace XamlingCore.iOS.Navigation
{
    public class iOSFrameManager
    {
        private readonly IViewResolver _viewResolver;

        public iOSFrameManager(IViewResolver viewResolver)
        {
            _viewResolver = viewResolver;
        }

        public UIViewController Init(XFrame rootFrame, XViewModel rootViewModel, XViewModel initalViewModel)
        {
            var rootPage = _viewResolver.Resolve(rootViewModel);
            var rootView = _viewResolver.Resolve(initalViewModel);

            if (rootPage == null || rootView == null)
            {
                throw new InvalidOperationException("Could not resolve the inital views");
            }

            _configureNavigation(rootFrame, rootView);

            return rootView.CreateViewController();
        }

        void _configureNavigation(XFrame rootFrame, Page rootPage)
        {
            if (rootPage is MasterDetailPage)
            {
                
            }

            if (rootPage is NavigationPage)
            {
                FrameNavigator = new iOSNavigationPageNavigator(rootFrame, rootPage as NavigationPage);
            }
            
        }

        public IFrameNavigator FrameNavigator { get; set; }
    }
}