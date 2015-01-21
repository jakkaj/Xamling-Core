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
    public class HomePageFourViewModel :XViewModel
    {
        public ICommand PreviousPageCommand { get; set; }

        public HomePageFourViewModel()
        {
            PreviousPageCommand = new Command(_navigateBack);
        }

        void _navigateBack()
        {

            NavigateBack();
        }
    }
}
