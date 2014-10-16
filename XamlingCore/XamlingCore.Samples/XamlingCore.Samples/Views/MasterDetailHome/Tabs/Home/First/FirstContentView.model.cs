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
            this.Title = "FirstPanel";
            NextPageCommand = new Command(_onNextPage);
        }

        private void _onNextPage()
        {
            NavigateTo<LoadersViewModel>();
        }
    }
}
