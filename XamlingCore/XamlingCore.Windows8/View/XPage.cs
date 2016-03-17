using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using XamlingCore.Portable.View.ViewModel;

namespace XamlingCore.Windows8.View
{
    public abstract class XPage : Page

    {
        public abstract void SetViewModel(object vm);

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (DataContext != e.Parameter)
            {
                DataContext = e.Parameter;
                SetViewModel(e.Parameter);
            }

            base.OnNavigatedTo(e);
        }
    }
}
