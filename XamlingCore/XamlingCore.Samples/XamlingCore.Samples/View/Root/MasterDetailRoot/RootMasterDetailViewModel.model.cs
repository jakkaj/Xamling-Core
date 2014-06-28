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
            //build the child pages
            //remember this vm + page combination hosts the actual MasterDetail view
            

            //add a couple of pages. These view models are the root items that then point off to the real items.
            AddPage(CreateContentModel<HomeViewModel>());
            AddPage(CreateContentModel<AnotherMenuOptionViewModel>());

            SetMaster(CreateContentModel<MasterDetailMenuPageViewModel>());

            Build();

            base.OnInitialise();
        }
    }
}
