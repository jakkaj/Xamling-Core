using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Autofac;

using XamlingCore.Portable.View.ViewModel;
using XamlingCore.Windows8.Contract;
using XamlingCore.XamarinThings.Contract;
using XamlingCore.XamarinThings.Navigators;
using XamlingCore.XamarinThings.UI;

namespace XamlingCore.Windows8.Core
{
    public class XUWPFrameManager : IXUWPFrameManager
    {
        private readonly ILifetimeScope _scope;
        private readonly IUWPViewResolver _viewResolver;


       

        public XUWPFrameManager(ILifetimeScope scope, IUWPViewResolver viewResolver)
        {
            _scope = scope;
            _viewResolver = viewResolver;
        }

        public void Init(XFrame rootFrame, XViewModel rootViewModel)
        {
            RootViewModel = rootViewModel;

            var rootPage = _viewResolver.ResolvePageType(rootViewModel);

            if (rootPage == null)
            {
                throw new InvalidOperationException("Could not resolve the inital views");
            }

            _configureNavigation(rootFrame, rootPage);

            rootFrame.Init();

            rootFrame.NavigateTo(rootViewModel);
        }

        void _configureNavigation(XFrame rootXFrame, Type rootPageType)
        {

            Frame rootFrame = Window.Current.Content as Frame;
           
            if (rootFrame == null)
            {
                rootFrame = new Frame();
                
                Window.Current.Content = rootFrame;
            }


            FrameNavigator = new XUWPFrameNavigator(_scope, rootXFrame, rootFrame, _viewResolver, rootPageType);

            

            // Ensure the current window is active
            Window.Current.Activate();

            //Navigations pages need navigators (which means the child VM's can navigate in them)
            //Pages like master details don't, their base VM's handle the changing of pages etc...
            //if (rootPage is NavigationPage)
            //{
            //    FrameNavigator = new XNavigationPageNavigator(_scope, rootFrame, rootPage as NavigationPage, _viewResolver);
            //}
        }

        

        public XUWPFrameNavigator FrameNavigator { get; set; }

        public XViewModel RootViewModel { get; private set; }
    }
}
