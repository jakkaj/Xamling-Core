using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using XamlingCore.Portable.View;
using XamlingCore.Portable.View.ViewModel;
using XamlingCore.Samples.UWP.Native.View.Home.Subs;

namespace XamlingCore.Samples.UWP.Native.View.Home
{
    public class HomeViewModel : XViewModel
    {
        public ICommand NextPageCommand { get; set; }
        public HomeViewModel()
        {
            NextPageCommand = new XCommand(_onNextPage);
        }

        void _onNextPage()
        {
            NavigateTo<SecondHomePageViewModel>();
        }
    }
}
