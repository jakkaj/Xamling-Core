using System.Windows.Input;
using Xamarin.Forms;
using XamlingCore.Portable.View.ViewModel;

namespace XamlingCore.Samples.Views.MasterDetailHome.Home
{
    public class HomeViewModel : XViewModel
    {
        public ICommand NextPageCommand { get; set; }

        public HomeViewModel()
        {
            Title = "Home";
            NextPageCommand = new Command(_nextPage);
        }

        public override void OnInitialise()
        {
            base.OnInitialise();
        }

        void _nextPage()
        {
            NavigateTo<HomePageTwoViewModel>();
        }
    }
}
