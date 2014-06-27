using System;
using Xamarin.Forms;
using XamlingCore.Portable.View.ViewModel;
using XamlingCore.XamarinThings.Contract;
using XamlingCore.XamarinThings.Navigators;
using XamlingCore.XamarinThings.ViewModel;

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

            _configureNavigation(rootFrame, rootViewModel, rootPage);

            rootFrame.Init();

            return rootPage;
        }

        void _configureNavigation(XFrame rootFrame, XViewModel rootViewModel, Page rootPage)
        {
            if (rootPage is MasterDetailPage)
            {
                //if (!(rootViewModel is XMasterDetailViewModel))
                //{
                //    throw new ArgumentException("Root view model must be XMasterDetailViewModelBase");;
                //}
                //FrameNavigator = new XNavigationMasterDetailNavigator(rootFrame, rootPage as MasterDetailPage, _viewResolver);
            }

            if (rootPage is NavigationPage)
            {
                FrameNavigator = new XNavigationPageNavigator(rootFrame, rootPage as NavigationPage, _viewResolver);
            }
        }

        public IFrameNavigator FrameNavigator { get; set; }

        public XViewModel RootViewModel { get; set; }
    }
}