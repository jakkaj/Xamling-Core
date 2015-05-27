using System;
using System.Linq;
using Autofac;
using Xamarin.Forms;
using XamlingCore.Portable.Contract.Navigation;
using XamlingCore.Portable.Model.Navigation;
using XamlingCore.Portable.View.ViewModel;
using XamlingCore.XamarinThings.Content.Navigation;
using XamlingCore.XamarinThings.Contract;
using XamlingCore.XamarinThings.Frame;

namespace XamlingCore.XamarinThings.Navigators
{
    public class XNavigationPageNavigator : IFrameNavigator
    {
        private readonly ILifetimeScope _scope;
        private readonly XFrame _rootFrame;
        private readonly IXNavigation _xNavigation;
        private readonly ILifetimeScope _container;

        private readonly NavigationPage _rootNavigationPage;
        private readonly IViewResolver _viewResolver;

        private INavigation _xamarinNavigation;



        public XNavigationPageNavigator(ILifetimeScope scope, XFrame rootFrame, NavigationPage page, IViewResolver viewResolver)
        {
            _scope = scope;
            _rootFrame = rootFrame;
            _xNavigation = rootFrame.Navigation;
            _container = rootFrame.Container;
            _rootNavigationPage = page;
            _viewResolver = viewResolver;

            _configure();


        }

        public void _configure()
        {
            _xNavigation.Navigated += _xNavigation_Navigated;

            _rootNavigationPage.Popped += _rootNavigationPage_Popped;
            _rootNavigationPage.PoppedToRoot += _rootNavigationPage_PoppedToRoot;
            _rootNavigationPage.Pushed += _rootNavigationPage_Pushed;

            _xamarinNavigation = _rootNavigationPage.Navigation;

            if (_xNavigation.CurrentContentObject != null)
            {
                _setView(NavigationDirection.Forward);
            }
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

        async void _navigationModal()
        {
            var vm = _xNavigation.ModalContentObject as XViewModel;

            if (vm == null)
            {
                await _xamarinNavigation.PopModalAsync();
                return;
            }

            var rootFrame = XFrame.CreateRootFrame<XRootFrame>(_scope);
            rootFrame.IsModal = true;

            var frameManager = _scope.Resolve<IFrameManager>();

            vm.ParentModel = rootFrame;

            var rootNavigationVm = rootFrame.CreateContentModel<XNavigationPageViewModel>();

            var initalViewController = frameManager.Init(rootFrame, rootNavigationVm);
            rootFrame.NavigateTo(vm);

            await _xamarinNavigation.PushModalAsync(initalViewController, true);
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

            var p = _viewResolver.Resolve(vm);

            if (p == null)
            {
                throw new Exception("View type not implemented");
            }

            await _xamarinNavigation.PushAsync(p);


            if (currentPage != null &&
                !_xNavigation.NavigationHistory.Contains(currentPage.BindingContext) &&
                _xamarinNavigation.NavigationStack.Contains(currentPage)
                )
            {
                _xamarinNavigation.RemovePage(currentPage);
            }
        }

        async void _navigationBackward()
        {
            var currentPage = _rootNavigationPage.CurrentPage;

            if (currentPage != null && currentPage.BindingContext != null && currentPage.BindingContext == _rootFrame.CurrentContentObject)
            {
                //This page is already correct (probably an out of XCore back)
                return;
            }

            do
            {
                var p = _xamarinNavigation.NavigationStack.LastOrDefault();
                var vm = p.BindingContext;
                var notCorrectVm = _rootFrame.CurrentContentObject != vm;

                if (!notCorrectVm)
                {
                    break;
                }

                _xamarinNavigation.RemovePage(p);
            } while (true);
            
            await _xamarinNavigation.PopAsync();
        }

        private void _setView(NavigationDirection direction)
        {
            if (direction == NavigationDirection.Forward)
            {
                _navigationForward();
            }
            else if (direction == NavigationDirection.Back)
            {
                _navigationBackward();
            }
            else
            {
                _navigationModal();
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