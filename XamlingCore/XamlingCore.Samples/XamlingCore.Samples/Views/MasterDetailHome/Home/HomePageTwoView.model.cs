using System.Windows.Input;
using Xamarin.Forms;
using XamlingCore.Portable.View.ViewModel;
using XamlingCore.Samples.Views.MasterDetailHome.AnotherMenuOption;

namespace XamlingCore.Samples.Views.MasterDetailHome.Home
{
    public class HomePageTwoViewModel : XViewModel
    {
        public ICommand PopCommand { get; set; }

        public ICommand NextCommand { get; set; }

        public HomePageTwoViewModel()
        {
            PopCommand = new Command(_onPop);
            NextCommand = new Command(_onNext);
        }

        void _onNext()
        {
            NavigateTo<AnotherMenuOptionViewModel>();
        }

        void _onPop()
        {
            NavigateToModal(null);
        }
    }
}
