using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using XamlingCore.Portable.View.ViewModel;

namespace XamlingCore.Samples.View.MasterDetailHome.Home
{
    public class HomeViewModel : XViewModel
    {
        public ICommand NextPageCommand { get; set; }

        public HomeViewModel()
        {
            Title = "Home";
            NextPageCommand = new Command(_nextPage);
        }

        void _nextPage()
        {
            NavigateTo<HomePageTwoViewModel>();
        }
    }
}
