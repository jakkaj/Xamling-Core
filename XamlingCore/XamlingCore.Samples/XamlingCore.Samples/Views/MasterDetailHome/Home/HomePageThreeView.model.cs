using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using XamlingCore.Portable.View.ViewModel;

namespace XamlingCore.Samples.Views.MasterDetailHome.Home
{
    public class HomePageThreeViewModel : XViewModel
    {
        public ICommand NextCommand { get; set; }

        public HomePageThreeViewModel()
        {
            NextCommand = new Command(_onNext);
        }

        void _onNext()
        {
            NavigateTo<HomePageFourViewModel>(null, true);
        }
    }
}
