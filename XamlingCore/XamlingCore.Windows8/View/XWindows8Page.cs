using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace XamlingCore.Windows8.View
{
    public class XWindows8Page : Page
    {
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (DataContext != e.Parameter)
            {
                DataContext = e.Parameter;
            }
            base.OnNavigatedTo(e);
        }
    }
}
