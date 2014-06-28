using System;
using Xamarin.Forms;
using XamlingCore.Portable.View.ViewModel;
using XamlingCore.XamarinThings.Contract;
using XamlingCore.XamarinThings.Navigators;

namespace XamlingCore.XamarinThings.Frame
{
    public class XFrameManager : IFrameManager
    {
        private readonly IViewResolver _viewResolver;

        public XFrameManager(IViewResolver viewResolver)
        {
            _viewResolver = viewResolver;
        }

        public Page Init(XFrame rootFrame, XViewModel rootViewModel)
        {
            RootViewModel = rootViewModel;
                
            var rootPage = _viewResolver.Resolve(rootViewModel);

            if (rootPage == null)
            {
                throw new InvalidOperationException("Could not resolve the inital views");
            }

            _configureNavigation(rootFrame, rootPage);

            rootFrame.Init();

            return rootPage;
        }

        void _configureNavigation(XFrame rootFrame, Page rootPage)
        {
            //Navigations pages need navigators (which means the child VM's can navigate in them)
            //Pages like master details don't, their base VM's handle the changing of pages etc...
            if (rootPage is NavigationPage)
            {
                FrameNavigator = new XNavigationPageNavigator(rootFrame, rootPage as NavigationPage, _viewResolver);
            }
        }

        public IFrameNavigator FrameNavigator { get; set; }

        public XViewModel RootViewModel { get; set; }
    }
}