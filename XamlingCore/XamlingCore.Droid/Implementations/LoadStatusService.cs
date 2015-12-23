using Android.App;
using Xamarin.Forms;
using XamlingCore.Portable.Contract.UI;
using XamlingCore.Portable.View.Special;

namespace XamlingCore.Droid.Implementations
{
    public class LoadStatusService : LoadStatusServiceBase
    {
        private ProgressDialog _spinnerInstance;

        public LoadStatusService(IDispatcher dispatcher) : base(dispatcher)
        {
        }

        public override void ShowIndicator(string text)
        {
            if (!string.IsNullOrWhiteSpace(text))
            {
                _hideTray();
                _showFullScreen(text);
            }
            else
            {
                if (_spinnerInstance == null)
                {
                    _hideFullScreen();
                    _showTray();    
                }
            }
        }

        public override void HideIndicator()
        {
            _hideFullScreen();
            _hideTray();
        }

        void _showFullScreen(string text)
        {
            if (_spinnerInstance == null)
            {
                _spinnerInstance = new ProgressDialog(Forms.Context);
                _spinnerInstance.Indeterminate = true;
                _spinnerInstance.SetProgressStyle(ProgressDialogStyle.Spinner);                
                _spinnerInstance.SetCancelable(false);                
            }
            
            _spinnerInstance.SetMessage(text);

            if (!_spinnerInstance.IsShowing)
            {
                try
                {
                    _spinnerInstance.Show();
                }
                catch { }
                
            }            
        }

        void _hideFullScreen()
        {
            if (_spinnerInstance != null)
            {
                _spinnerInstance.Hide();
            }

            _spinnerInstance = null;
        }

        void _hideTray()
        {
            //TODO: Required on droid??
        }

        void _showTray()
        {
            //TODO: Required on droid??
        }
    }
}