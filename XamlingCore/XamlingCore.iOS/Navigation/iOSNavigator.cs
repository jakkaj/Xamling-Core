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

namespace XamlingCore.iOS.Navigation
{
    public class iOSNavigator
    {
        private readonly XRootViewModelBase _rootVm;
        private readonly IXNavigation _xNavigation;
        private readonly ILifetimeScope _container;

        private UIWindow _window;
        private NavigationPage _rootNavigationPage;
        private INavigation _xamarinNavigation;
        public iOSNavigator(XRootViewModelBase rootVm, IXNavigation xNavigation, ILifetimeScope container)
        {
            _rootVm = rootVm;
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

            _setView(NavigationDirection.Forward);
        }

        async void _navigationForward()
        {

            var vm = _xNavigation.CurrentContentObject;

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
            await _xamarinNavigation.PopAsync();
        }

        private async void _setView(NavigationDirection direction)
        {
            while (!_rootVm.IsReady)
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
    }
}