using Autofac;
using XamlingCore.Portable.Contract.Localisation;
using XamlingCore.Portable.Contract.Navigation;
using XamlingCore.Portable.Contract.Services;
using XamlingCore.Portable.Contract.UI;
using XamlingCore.Portable.View.ViewModel;

namespace XamlingCore.UWP.Core
{
    public class XUWPRootFrame : XFrame
    {
        public XUWPRootFrame(ILifetimeScope c, ILoadStatusService systemTrayService, IOrientationService orientationService, ILocalisationService localisationService, IXNavigation xNavigationService, IDispatcher dispatcher) : base(c, systemTrayService, orientationService, localisationService, xNavigationService, dispatcher)
        {
        }

        public override void Init()
        {
            SetReady();
        }
    }
}
