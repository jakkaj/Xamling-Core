using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XamlingCore.Samples.UWP.Native.View.AnotherMenu;
using XamlingCore.Samples.UWP.Native.View.Home;
using XamlingCore.Samples.UWP.Native.View.Menu;
using XamlingCore.UWP.Contract;
using XamlingCore.UWP.Navigation.MasterDetail;

namespace XamlingCore.Samples.UWP.Native.View
{
    public class RootViewModel : XUWPMasterDetailViewModel
    {
        public RootViewModel(IUWPViewResolver viewResolver) : base(viewResolver)
        {
        }

        public override void OnInitialise()
        {
            AddPackage<HomeViewModel>();
            AddPackage<AnotherPageViewModel>();

            SetMaster(CreateContentModel<MenuMasterViewModel>());

            Build();

            base.OnInitialise();
        }
    }
}
