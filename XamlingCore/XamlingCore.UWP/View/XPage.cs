using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace XamlingCore.UWP.View
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
