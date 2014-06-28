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
//    /// <summary>
//    /// Use this frame when you want the root area to be a MasterDetailpage
//    /// Couple this with a RootViewModel base type of XMasterDetailViewModel
//    /// and a root page type of XMasterDetailView
//    /// </summary>
//    public class MasterDetailRootFrame : XFrame
//    {
//        public MasterDetailRootFrame(ILifetimeScope c,
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
//            SetReady();
//        }
//    }
//}
