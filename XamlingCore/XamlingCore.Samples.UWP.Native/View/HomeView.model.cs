using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;
using Xamarin.Forms;
using XamlingCore.Portable.View.ViewModel;

namespace XamlingCore.Samples.UWP.Native.View
{
    public class HomeViewModel : XViewModel
    {

       

        public HomeViewModel()
        {
          
        }

        public void DoNavigate(object sender, RoutedEventArgs args)
        {
            NavigateTo<AnotherViewModel>();
        }

        public string Test
        {
            get
            {
                return "Jordan";
            }
        }
    }
}
