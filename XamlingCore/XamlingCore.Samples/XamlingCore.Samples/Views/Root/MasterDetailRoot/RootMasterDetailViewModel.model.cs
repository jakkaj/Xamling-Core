using Xamarin.Forms;
using XamlingCore.Samples.Views.Home.Playground;
using XamlingCore.Samples.Views.MasterDetailHome.AnotherMenuOption;
using XamlingCore.Samples.Views.MasterDetailHome.GoogleAnalytics;
using XamlingCore.Samples.Views.MasterDetailHome.Home;
using XamlingCore.Samples.Views.MasterDetailHome.List;
using XamlingCore.Samples.Views.MasterDetailHome.Loaders;
using XamlingCore.Samples.Views.MasterDetailHome.Location;
using XamlingCore.Samples.Views.MasterDetailHome.Menu;
using XamlingCore.Samples.Views.MasterDetailHome.Orientation;
using XamlingCore.Samples.Views.MasterDetailHome.Tabs;
using XamlingCore.XamarinThings.Content.MasterDetail;
using XamlingCore.XamarinThings.Contract;

namespace XamlingCore.Samples.Views.Root.MasterDetailRoot
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
            AddPage(CreateContentModel<LocationViewModel>());
            AddPage(CreateContentModel<LoadersViewModel>());
            AddPage(CreateContentModel<PlaygroundHomeViewModel>());
            AddPage(CreateContentModel<OrientationViewModel>());
            AddPage(CreateContentModel<LongListViewModel>());

            //Add the master view (which is like the menu bit the flies out)
            SetMaster(CreateContentModel<MasterDetailMenuPageViewModel>());

            Build();

            base.OnInitialise();
        }
    }
}
