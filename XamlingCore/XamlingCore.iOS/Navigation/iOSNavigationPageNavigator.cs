using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Core;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Xamarin.Forms;

using XamlingCore.Portable.Contract.Navigation;
using XamlingCore.Portable.Model.Navigation;
using XamlingCore.Portable.View.ViewModel;
using XamlingCore.Xamarin.Contract;

namespace XamlingCore.iOS.Navigation
{
    public class iOSNavigationPageNavigator : IFrameNavigator
    {
        private readonly XFrame _rootFrame;
        private readonly IXNavigation _xNavigation;
        private readonly ILifetimeScope _container;

        private readonly NavigationPage _rootNavigationPage;
        
        private INavigation _xamarinNavigation;

        public iOSNavigationPageNavigator(XFrame rootFrame, NavigationPage page)
        {

            _rootFrame = rootFrame;
            _xNavigation = rootFrame.Navigation;
            _container = rootFrame.Container;
            _rootNavigationPage = page;

            _configure();
        }

        void _configure()
        {
            _xNavigation.Navigated += _xNavigation_Navigated;
            
            _rootNavigationPage.Popped += _rootNavigationPage_Popped;
            _rootNavigationPage.PoppedToRoot += _rootNavigationPage_PoppedToRoot;
            _rootNavigationPage.Pushed += _rootNavigationPage_Pushed;
            
            _xamarinNavigation = _rootNavigationPage.Navigation;
        }

        void _rootNavigationPage_Pushed(object sender, NavigationEventArgs e)
        {
            _synchroniseNavigation(NavigationDirection.Forward);
        }

        void _rootNavigationPage_PoppedToRoot(object sender, NavigationEventArgs e)
        {
            _popRoot();
        }

        void _rootNavigationPage_Popped(object sender, NavigationEventArgs e)
        {
            _synchroniseNavigation(NavigationDirection.Back);
        }

        void _popRoot()
        {
            _rootFrame.NavigateHome();
        }

        /// <summary>
        /// Check that the current page is synchronised with the XCore navigation framework
        /// They can get our of wack as navigation can be kicked off by things outside the framework
        /// ... like the default back button in the NavigationPage
        /// </summary>
        /// <param name="direction"></param>
        void _synchroniseNavigation(NavigationDirection direction)
        {            
            var page = _rootNavigationPage.CurrentPage;

            if (page != null && page.BindingContext != null)
            {
                if (page.BindingContext != _xNavigation.CurrentContentObject)
                {
                    if (direction == NavigationDirection.Back)
                    {
                        _rootFrame.NavigateBack();
                    }
                    else
                    {
                        _rootFrame.NavigateTo(page.BindingContext);
                    }
                }    
            }
        }

        async void _navigationForward()
        {

            var vm = _xNavigation.CurrentContentObject;

            var currentPage = _rootNavigationPage.CurrentPage;

            if (currentPage != null && currentPage.BindingContext != null && currentPage.BindingContext == vm)
            {
                //This page is already correct (probably an out of XCore back)
                return;
            }

            var t = vm.GetType();

            var typeName = t.FullName.Replace("ViewModel", "View");

            //Xamarin Forms will resolve this way
            var nextToType = Type.GetType(string.Format("{0}, {1}", typeName, t.Assembly.FullName));

            if (_container.IsRegistered(nextToType))
            {
                var tUiView = _container.Resolve(nextToType) as Page;
                if (tUiView != null)
                {
                    tUiView.BindingContext = vm;
                    await _xamarinNavigation.PushAsync(tUiView);
                    return;
                }
            }

            throw new Exception("View type not implemented");

            //locate the view...

        }

        async void _navigationBackward()
        {
            var currentPage = _rootNavigationPage.CurrentPage;

            if (currentPage != null && currentPage.BindingContext != null && currentPage.BindingContext == _rootFrame.CurrentContentObject)
            {
                //This page is already correct (probably an out of XCore back)
                return;
            }

            await _xamarinNavigation.PopAsync();
        }

        private async void _setView(NavigationDirection direction)
        {
            while (!_rootFrame.IsReady)
            {
                await Task.Yield();
            }

            if (direction == NavigationDirection.Forward)
            {
                _navigationForward();
            }
            else
            {
                _navigationBackward();
            }


        }

        void _xNavigation_Navigated(object sender, XNavigationEventArgs e)
        {
            _setView(e.Direction);
        }

        public Page Page
        {
            get { return _rootNavigationPage; }
        }
    }
}