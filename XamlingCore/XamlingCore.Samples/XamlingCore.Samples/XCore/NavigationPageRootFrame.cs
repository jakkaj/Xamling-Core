//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Autofac;
//using XamlingCore.Portable.Contract.Localisation;
//using XamlingCore.Portable.Contract.Navigation;
//using XamlingCore.Portable.Contract.Services;
//using XamlingCore.Portable.Contract.UI;
//using XamlingCore.Portable.View.ViewModel;
//using XamlingCore.Samples.View.Home;

//namespace XamlingCore.Samples.XCore
//{
//    public class NavigationPageRootFrame : XFrame 
//    {
//        public NavigationPageRootFrame(ILifetimeScope c, 
//            ILoadStatusService loadStatusService, 
//            IOrientationService orientationService, 
//            ILocalisationService localisationService, 
//            IXNavigation xNavigationService,
//            IDispatcher dispatcher) : 
//            base(c, loadStatusService, 
//            orientationService, 
//            localisationService, 
//            xNavigationService,
//            dispatcher)
//        {
//        }

//        public override void Init()
//        {
//            NavigateTo(CreateContentModel<MainNavigationHomeViewModel>());
            
//            SetReady();
//        }
//    }
//}
