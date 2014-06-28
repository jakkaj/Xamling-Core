using XamlingCore.Samples.View.MasterDetailHome;
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
            AddPage(CreateContentModel<HomeItemViewModel>());
            AddPage(CreateContentModel<AnotherItemViewModel>());

            SetMaster(CreateContentModel<MasterDetailMenuPageViewModel>());

            Build();

            base.OnInitialise();
        }
    }
}
