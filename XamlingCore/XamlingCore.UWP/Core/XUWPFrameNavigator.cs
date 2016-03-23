using System;
using System.Linq;
using Windows.UI.Core;
using Windows.UI.Xaml.Navigation;
using Autofac;
using XamlingCore.Portable.Contract.Navigation;
using XamlingCore.Portable.Model.Navigation;
using XamlingCore.Portable.View.ViewModel;
using XamlingCore.UWP.Contract;
using XamlingCore.UWP.View;
using Frame = Windows.UI.Xaml.Controls.Frame;

namespace XamlingCore.UWP.Core
{
    public class XUWPFrameNavigator
    {
        private readonly ILifetimeScope _scope;
        private readonly XFrame _rootFrame;
        private readonly Frame _rootElement;
        private readonly IXNavigation _xNavigation;
        private readonly ILifetimeScope _container;

      
        private readonly IUWPViewResolver _viewResolver;

       

        public XUWPFrameNavigator(ILifetimeScope scope, XFrame rootFrame, Frame rootElement, IUWPViewResolver viewResolver, Type rootPageType)
        {
            _scope = scope;
            _rootFrame = rootFrame;
            _rootElement = rootElement;
            _xNavigation = rootFrame.Navigation;
            _container = rootFrame.Container;
            
            _viewResolver = viewResolver;

            _configure(rootPageType);


        }

        public void _configure(Type rootPageType)
        {
            _xNavigation.Navigated += _xNavigation_Navigated;

            _rootElement.Navigating += _rootElement_Navigating;

            if (_xNavigation.CurrentContentObject != null)
            {
                _setView(NavigationDirection.Forward);
                _navigationForward();
            }
        }

        private void _rootElement_Navigating(object sender, Windows.UI.Xaml.Navigation.NavigatingCancelEventArgs e)
        {
            if (e.NavigationMode == NavigationMode.Forward || e.NavigationMode == NavigationMode.New)
            {
                _synchroniseNavigation(NavigationDirection.Forward);
            }
            else if (e.NavigationMode == NavigationMode.Back)
            {
                _synchroniseNavigation(NavigationDirection.Back);
            }
        }
        
        /// <summary>
        /// Check that the current page is synchronised with the XCore navigation framework
        /// They can get our of wack as navigation can be kicked off by things outside the framework
        /// ... like the default back button in the NavigationPage
        /// </summary>
        /// <param name="direction"></param>
        void _synchroniseNavigation(NavigationDirection direction)
        {
            var page = _rootElement.Content as XPage;
            

            if (page != null && page.DataContext != null)
            {
                if (page.DataContext != _xNavigation.CurrentContentObject)
                {
                    if (direction == NavigationDirection.Back)
                    {
                        _rootFrame.NavigateBack();
                    }
                    else
                    {
                        _rootFrame.NavigateTo(page.DataContext);
                    }
                }
            }
        }

        async void _navigationModal()
        {
            //need to use ContentDialog here

            //var vm = _xNavigation.ModalContentObject as XViewModel;

            //if (vm == null)
            //{
            //    var mRoot = _xamarinNavigation.ModalStack?.LastOrDefault();

            //    if (mRoot != null)
            //    {
            //        var iPopOut = mRoot as XNavigationPageView;
            //        if (iPopOut != null)
            //        {
            //            await iPopOut.AnimateOut();
            //        }
            //    }
            //    await _xamarinNavigation.PopModalAsync();

            //    return;
            //}

            //var rootFrame = XFrame.CreateRootFrame<XRootFrame>(_scope);
            //rootFrame.IsModal = true;

            //var frameManager = _scope.Resolve<IFrameManager>();

            //vm.ParentModel = rootFrame;

            //var rootNavigationVm = rootFrame.CreateContentModel<XNavigationPageViewModel>();

            //var initalViewController = frameManager.Init(rootFrame, rootNavigationVm);



            //rootFrame.NavigateTo(vm);

            //var i = initalViewController as XNavigationPageView;
            //if (i != null)
            //{
            //    i.PrepForAnimation();
            //}

            //await _xamarinNavigation.PushModalAsync(initalViewController, true);

            //if (i != null)
            //{
            //    i.AnimateIn();
            //}
        }

        async void _navigationForward()
        {
            
            var vm = _xNavigation.CurrentContentObject;

            var currentPage = _rootElement.Content as XPage;

            if (currentPage != null && currentPage.DataContext != null && currentPage.DataContext == vm)
            {
                //This page is already correct (probably an out of XCore back)
                return;
            }

            var p = _viewResolver.ResolvePageType(vm);

            if (p == null)
            {
                throw new Exception("View type not implemented");
            }

            _rootElement.Navigate(p, _xNavigation.CurrentContentObject);

           


            if (currentPage != null &&
                !_xNavigation.NavigationHistory.Contains(currentPage.DataContext) &&
                    _rootElement.BackStack.Any(_=>_.Parameter == currentPage.DataContext)
                )
            {
                var itemsToRemove = _rootElement.BackStack.Where(_ => _.Parameter == currentPage.DataContext);
                foreach (var item in itemsToRemove)
                {
                    _rootElement.BackStack.Remove(item);
                }
            }
        }

        async void _navigationBackward()
        {
            var currentPage = _rootElement.Content as XPage;

            if (currentPage != null && currentPage.DataContext != null && currentPage.DataContext == _rootFrame.CurrentContentObject)
            {
                //This page is already correct (probably an out of XCore back)
                return;
            }

            do
            {
                if (!_rootElement.CanGoBack)
                {
                    break;
                }

                var p = _rootElement.BackStack.LastOrDefault();
                var vm = p.Parameter;
                var notCorrectVm = _rootFrame.CurrentContentObject != vm;

                if (!notCorrectVm)
                {
                    break;
                }

            } while (true);

            _rootElement.GoBack();
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
    }
}
