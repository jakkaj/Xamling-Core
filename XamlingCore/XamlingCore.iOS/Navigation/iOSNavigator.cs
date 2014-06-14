using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using Autofac.Core;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Xamarin.Forms;
using XamlingCore.Portable.Contract.Navigation;

namespace XamlingCore.iOS.Navigation
{
    public class iOSNavigator
    {
        private readonly IXNavigation _xNavigation;
        private readonly ILifetimeScope _container;

        private UIWindow _window;
        private NavigationPage _rootNavigationPage;
        private INavigation _xamarinNavigation;
        public iOSNavigator(IXNavigation xNavigation, ILifetimeScope container)
        {
            _xNavigation = xNavigation;
            _container = container;


            _configure();
        }

        void _configure()
        {
            _xNavigation.Navigated += _xNavigation_Navigated;

            _window = new UIWindow(UIScreen.MainScreen.Bounds);
            
            _rootNavigationPage = new NavigationPage();
            _xamarinNavigation = _rootNavigationPage.Navigation;

            _window.RootViewController = _rootNavigationPage.CreateViewController();
            _window.MakeKeyAndVisible();   
 
            _setView();
        }

        private void _setView()
        {
            var vm = _xNavigation.CurrentContentObject;
            var t = vm.GetType();

            var typeName = t.Name.Replace("ViewModel", "View");

            //Xamarin Forms will resolve this way
            var nextToType = Type.GetType(string.Format("{0}, {1}", typeName, t.Assembly.FullName));

            if (_container.IsRegistered(nextToType))
            {
                var tUiView = _container.Resolve(nextToType) as Page;
                if (tUiView != null)
                {
                    _xamarinNavigation.PushAsync(tUiView);
                }
            }

            throw new Exception("View type not implemented");

            //locate the view...


        }

        void _xNavigation_Navigated(object sender, EventArgs e)
        {
            _setView();
        }
    }
}