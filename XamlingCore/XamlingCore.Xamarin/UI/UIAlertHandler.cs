using System.Threading.Tasks;
using Xamarin.Forms;
using XamlingCore.Portable.Messages.View;
using XamlingCore.Portable.Messages.XamlingMessenger;
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
            new DisplayAlertMessage().Send();
            if (cancel == null)
            {
                await _p.DisplayAlert(title, message, accept);
                return true;
            }

            var result = await _p.DisplayAlert(title, message, accept, cancel);
            return result;
        }
    }
}
