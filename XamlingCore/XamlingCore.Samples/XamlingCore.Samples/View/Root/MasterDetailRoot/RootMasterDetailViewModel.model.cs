using XamlingCore.Samples.View.MasterDetailHome;
using XamlingCore.Samples.View.MasterDetailHome.Home.Root;
using XamlingCore.XamarinThings.Contract;
using XamlingCore.XamarinThings.ViewModel;

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
            

            //add a couple of pages. They just need to be ViewModels at this stage.
            AddPage(CreateContentModel<MasterDetailHomeRootViewModel>());
            AddPage(CreateContentModel<AnotherItemViewModel>());

            SetMaster(CreateContentModel<HomeMasterViewModel>());

            Build();

            base.OnInitialise();
        }
    }
}
