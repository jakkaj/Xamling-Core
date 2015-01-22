using System.Windows.Input;
using Xamarin.Forms;
using XamlingCore.Portable.View.ViewModel;
using XamlingCore.Samples.Views.MasterDetailHome.Loaders;

namespace XamlingCore.Samples.Views.MasterDetailHome.Tabs.Home.First
{
    public class FirstContentViewModel : XViewModel
    {
        public ICommand NextPageCommand { get; set; }
        public FirstContentViewModel()
        {
           
            NextPageCommand = new Command(_onNextPage);
        }

        public override void OnInitialise()
        {
            this.Title = "FirstPanel";
            base.OnInitialise();
        }

        private void _onNextPage()
        {
            NavigateTo<LoadersViewModel>();
        }
    }
}
