using Xamarin.Forms;
using XamlingCore.Samples.View.MasterDetailHome;
using XamlingCore.Samples.View.MasterDetailHome.AnotherMenuOption;
using XamlingCore.Samples.View.MasterDetailHome.Home;
using XamlingCore.Samples.View.MasterDetailHome.Menu;
using XamlingCore.XamarinThings.Content.MasterDetail;
using XamlingCore.XamarinThings.Contract;

namespace XamlingCore.Samples.View.Root.MasterDetailRoot
{
    public class RootMasterDetailViewModel : XMasterDetailViewModel
    {
        public RootMasterDetailViewModel(IViewResolver viewResolver) : base(viewResolver)
        {
        }

        public override void OnInitialise()
        {
            NavigationTint = Color.Silver;

            //add a couple of pages.
            AddPage(CreateContentModel<HomeViewModel>());
            AddPage(CreateContentModel<AnotherMenuOptionViewModel>());

            //Add the master view (which is like the menu bit the flies out)
            SetMaster(CreateContentModel<MasterDetailMenuPageViewModel>());

            Build();

            base.OnInitialise();
        }
    }
}
