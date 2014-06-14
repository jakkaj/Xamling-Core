using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using XamlingCore.Portable.Contract.Localisation;
using XamlingCore.Portable.Contract.Navigation;
using XamlingCore.Portable.Contract.Services;
using XamlingCore.Portable.View.ViewModel;

namespace XamlingCore.Samples.XCore
{
    public class RootViewModel : XRootViewModelBase 
    {
        public RootViewModel(ILifetimeScope c, 
            ILoadStatusService systemTrayService, 
            IApplicationBarService applicationBarService, 
            IOrientationService orientationService, 
            ILocalisationService localisationService, 
            IXNavigationService xNavigationService) : 
            base(c, systemTrayService, 
            applicationBarService, 
            orientationService, 
            localisationService, 
            xNavigationService)
        {
        }

        public async override Task Init()
        {
            
        }
    }
}
