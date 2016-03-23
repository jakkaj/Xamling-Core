using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Autofac;
using XamlingCore.Portable.Contract.UI;
using XamlingCore.Portable.View.ViewModel;
using XamlingCore.UWP.Contract;

namespace XamlingCore.UWP.Core
{
    public class XUWPFrameManager : IXUWPFrameManager
    {
        private readonly ILifetimeScope _scope;
        private readonly IUWPViewResolver _viewResolver;
        private readonly IDispatcher _dispatcher;


        public XUWPFrameManager(ILifetimeScope scope, IUWPViewResolver viewResolver, IDispatcher dispatcher)
        {
            _scope = scope;
            _viewResolver = viewResolver;
            _dispatcher = dispatcher;
        }

        public UIElement Init(XFrame rootFrame, XViewModel rootViewModel, bool isRoot)
        {
            RootViewModel = rootViewModel;

            var rootPage = _viewResolver.ResolvePageType(rootViewModel);

            if (rootPage == null)
            {
                throw new InvalidOperationException("Could not resolve the inital views");
            }

            UIElement rootElement = _configureNavigation(rootFrame, rootPage, isRoot);

            rootFrame.Init();

            if (isRoot)
            {
                _dispatcher.Invoke(() => rootFrame.NavigateTo(rootViewModel));
            }

            return rootElement;
        }

        UIElement _configureNavigation(XFrame rootXFrame, Type rootPageType, bool isRoot)
        {
            Frame rootFrame = null;

            if (isRoot)
            {
                 rootFrame = Window.Current.Content as Frame;
            }

            if (rootFrame == null)
            {
                rootFrame = new Frame();
            }

            if (isRoot)
            {
                Window.Current.Content = rootFrame;
            }

            FrameNavigator = new XUWPFrameNavigator(_scope, rootXFrame, rootFrame, _viewResolver, rootPageType);

            // Ensure the current window is active
            if (isRoot)
            {
                Window.Current.Activate();
            }
          

            return rootFrame;
        }

        

        public XUWPFrameNavigator FrameNavigator { get; set; }

        public XViewModel RootViewModel { get; private set; }
    }
}
