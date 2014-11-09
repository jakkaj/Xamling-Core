using System.Threading.Tasks;
using Xamarin.Forms;
using XamlingCore.Portable.View.ViewModel;

namespace XamlingCore.XamarinThings.UI
{
    public class FormsAlertHandler
    {
        private readonly Page _p;

        public FormsAlertHandler(Page p)
        {
            _p = p;
            XViewModel.NativeAlert = DisplayAlert;
        }

        public async Task<bool> DisplayAlert(string title, string message, string accept, string cancel)
        {
            var result = await _p.DisplayAlert(title, message, accept, cancel);
            return result;
        }
    }
}
