using System.Windows.Input;
using Xamarin.Forms;
using XamlingCore.Portable.View.ViewModel;

namespace XamlingCore.Samples.Views.Home
{
    public class MainNavigationHomeViewModel : XViewModel
    {
        private string _mainText;

        public ICommand NextPageCommand { get; set; }

        public MainNavigationHomeViewModel()
        {
            NextPageCommand = new Command(_onNextPage);
        }
        
        public override void OnInitialise()
        {
            MainText = "This is some main text!";

            

            base.OnInitialise();
        }
        void _onNextPage()
        {
            NavigateTo<SecondNavigationViewModel>();
        }

        public string MainText
        {
            get { return _mainText; }
            set
            {
                _mainText = value;
                OnPropertyChanged();
            }
        }
    }
}
