using Autofac;
using Xamarin.Forms;
using XamlingCore.Portable.Contract.Navigation;
using XamlingCore.Portable.View.ViewModel;
using XamlingCore.XamarinThings.Contract;

namespace XamlingCore.XamarinThings.Navigators
{
    public class XNavigationMasterDetailNavigator : IFrameNavigator
    {
        public Page Page { get; private set; }
        
        private readonly XFrame _rootFrame;
        private readonly IXNavigation _xNavigation;
        private readonly ILifetimeScope _container;

        private readonly MasterDetailPage _rootNavigationPage;
        private readonly IViewResolver _viewResolver;

        private INavigation _xamarinNavigation;

        public XNavigationMasterDetailNavigator(XFrame rootFrame, MasterDetailPage page, IViewResolver viewResolver)
        {
            _rootFrame = rootFrame;
            _xNavigation = rootFrame.Navigation;
            _container = rootFrame.Container;
            _rootNavigationPage = page;
            _viewResolver = viewResolver;

            _configure();
        }

        void _configure()
        {
            
        }
    }
}