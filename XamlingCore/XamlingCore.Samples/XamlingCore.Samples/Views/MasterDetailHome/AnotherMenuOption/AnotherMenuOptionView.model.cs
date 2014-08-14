using System.Windows.Input;
using Xamarin.Forms;
using XamlingCore.Portable.View.ViewModel;

namespace XamlingCore.Samples.Views.MasterDetailHome.AnotherMenuOption
{
    public class AnotherMenuOptionViewModel : XViewModel
    {
        public ICommand PopCommand { get; set; }
        public AnotherMenuOptionViewModel()
        {
            Title = "Another option";
            PopCommand = new Command(_onPop);
        }


        void _onPop()
        {
            NavigateToModal(null);
        }
    }
}
