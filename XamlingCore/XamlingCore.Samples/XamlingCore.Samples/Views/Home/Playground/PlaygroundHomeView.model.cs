using System.Windows.Input;
using Xamarin.Forms;
using XamlingCore.Portable.View.ViewModel;
using XamlingCore.Samples.Views.Home.Playground.ViewTransitions;

namespace XamlingCore.Samples.Views.Home.Playground
{
    public class PlaygroundHomeViewModel : XViewModel
    {
        public ICommand GoToTransCommand { get; set; }

        public PlaygroundHomeViewModel()
        {
            Title = "Playground";

            GoToTransCommand = new Command(_goToTrans);
        }

        void _goToTrans()
        {
            NavigateTo<TransitionsTestViewModel>();
        }
    }
}
