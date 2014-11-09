using System;
using Autofac;
using Xamarin.Forms;
using XamlingCore.Portable.View.ViewModel;
using XamlingCore.XamarinThings.Contract;
using XamlingCore.XamarinThings.Navigators;
using XamlingCore.XamarinThings.UI;

namespace XamlingCore.XamarinThings.Frame
{
    public class XFrameManager : IFrameManager
    {
        private readonly ILifetimeScope _scope;
        private readonly IViewResolver _viewResolver;

        private FormsAlertHandler _alertHandler;

        public XFrameManager(ILifetimeScope scope, IViewResolver viewResolver)
        {
            _scope = scope;
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
            _configureAlerts(rootPage);
            
            rootFrame.Init();

            return rootPage;
        }

        void _configureNavigation(XFrame rootFrame, Page rootPage)
        {
            //Navigations pages need navigators (which means the child VM's can navigate in them)
            //Pages like master details don't, their base VM's handle the changing of pages etc...
            if (rootPage is NavigationPage)
            {
                FrameNavigator = new XNavigationPageNavigator(_scope, rootFrame, rootPage as NavigationPage, _viewResolver);
            }
        }

        void _configureAlerts(Page rootPage)
        {
            _alertHandler = new FormsAlertHandler(rootPage);
        }

        public IFrameNavigator FrameNavigator { get; set; }

        public XViewModel RootViewModel { get; set; }
    }
}