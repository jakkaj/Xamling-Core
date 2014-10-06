using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XamlingCore.Samples.Views.MasterDetailHome.Home;
using XamlingCore.Samples.Views.MasterDetailHome.Tabs.Home;
using XamlingCore.Samples.Views.MasterDetailHome.Tabs.Home.First;
using XamlingCore.Samples.Views.MasterDetailHome.Tabs.Home.Second;
using XamlingCore.XamarinThings.Content.TabbedPages;

namespace XamlingCore.Samples.Views.MasterDetailHome.Tabs
{
    public class HomeTabsViewModel : XTabbedPageViewModel
    {
        public HomeTabsViewModel()
        {
            Title = "Home";
        }
        public override void OnInitialise()
        {
            AddPage(CreateContentModel<FirstContentViewModel>());
            AddPage(CreateContentModel<SecondContentViewModel>());
            base.OnInitialise();
        }
    }
}
