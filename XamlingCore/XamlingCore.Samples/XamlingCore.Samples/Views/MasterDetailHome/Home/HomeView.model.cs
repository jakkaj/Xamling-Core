using System.Windows.Input;
using Xamarin.Forms;
using XamlingCore.Portable.Messages.View;
using XamlingCore.Portable.Messages.XamlingMessenger;
using XamlingCore.Portable.View.ViewModel;

namespace XamlingCore.Samples.Views.MasterDetailHome.Home
{
    public class HomeViewModel : XViewModel
    {
        public ICommand NextPageCommand { get; set; }
        public ICommand ShowNativeViewCommand { get; set; }
        public HomeViewModel()
        {
            Title = "Home";
            NextPageCommand = new Command(_nextPage);
            ShowNativeViewCommand = new Command(_onShowNativeView);
        }

        public override void OnInitialise()
        {
            base.OnInitialise();
        }

        void _onShowNativeView()
        {
            new ShowNativeViewMessage("XamlingCore.Samples.iOS.NativeViews.SomeNativeView").Send();
            //new ShowNativeViewMessage("NativeStoryboardView").Send();
        }

        void _nextPage()
        {
            //NavigateTo<HomePageTwoViewModel>();
            NavigateToModal<HomePageTwoViewModel>();
        }
    }
}
