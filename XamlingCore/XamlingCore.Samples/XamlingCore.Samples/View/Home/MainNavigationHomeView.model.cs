using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using XamlingCore.Portable.View.ViewModel;
using XamlingCore.Samples.View.MasterDetailHome;

namespace XamlingCore.Samples.View.Home
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
