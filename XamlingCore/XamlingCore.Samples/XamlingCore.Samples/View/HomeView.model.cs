using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using XamlingCore.Portable.View.ViewModel;

namespace XamlingCore.Samples.View
{
    public class HomeViewModel :XViewModel
    {
        private string _mainText;

        public ICommand NextPageCommand { get; set; }

        public override void OnInitialise()
        {
            MainText = "This is some main text!";

            NextPageCommand = new Command(_onNextPage);

            base.OnInitialise();
        }

        void _onNextPage()
        {
            NavigateTo<NextPageViewModel>();
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
