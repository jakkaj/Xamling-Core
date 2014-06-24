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
    public class NextPageViewModel : XViewModel
    {
        public ICommand NavigateBackCommand { get; set; }

        public NextPageViewModel()
        {
            NavigateBackCommand = new Command(_doBackwardsNavigation);
        }

        void _doBackwardsNavigation()
        {
            NavigateBack();
        }
    }
}
