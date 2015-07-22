using UIKit;
using XamlingCore.iOS.Unified.Controls.Loader;
using XamlingCore.iOS.Unified.Root;
using XamlingCore.Portable.Contract.UI;
using XamlingCore.Portable.Messages.View;
using XamlingCore.Portable.Messages.XamlingMessenger;
using XamlingCore.Portable.View.Special;

namespace XamlingCore.iOS.Unified.Implementations
{
    public class LoadStatusService : LoadStatusServiceBase
    {
        private LoadingOverlayViewBase _spinnerInstance;

        public static bool OverrideDefaultLoader { get; set; }

        public LoadStatusService(IDispatcher dispatcher) : base(dispatcher)
        {
        }

        public override void ShowIndicator(string text)
        {
            new SetLoaderMessage(text).Send();

            if (OverrideDefaultLoader)
            {
                return;
            }

            if (!string.IsNullOrWhiteSpace(text))
            {
                _hideTray();
                _showFullScreen(text);
            }
            else
            {
                //don't show the tray loader if the full screen spinner is up
                if (_spinnerInstance == null)
                {
                    _hideFullScreen();
                    _showTray();    
                }
            }
        }

        public override void HideIndicator()
        {
            new HideLoaderMessage().Send();
            _hideFullScreen();
            _hideTray();
        }

        void _showFullScreen(string text)
        {
            if (_spinnerInstance != null)
            {
                _spinnerInstance.Text = text;
                return;
            }

            _spinnerInstance = new LoadingOverlayView(UIScreen.MainScreen.Bounds);

            _spinnerInstance.Text = text;

            XiOSRoot.RootWindow.AddSubview(_spinnerInstance);
        }

        void _hideFullScreen()
        {
            if (_spinnerInstance != null)
            {
               _spinnerInstance.RemoveFromSuperview();
            }

            _spinnerInstance = null;
        }

        void _hideTray()
        {
            UIApplication.SharedApplication.NetworkActivityIndicatorVisible = false;
        }

        void _showTray()
        {
            UIApplication.SharedApplication.NetworkActivityIndicatorVisible = true;
        }

        
    }
}