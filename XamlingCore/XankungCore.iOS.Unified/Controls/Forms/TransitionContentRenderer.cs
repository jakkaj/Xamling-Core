using System.Threading.Tasks;
using CoreGraphics;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using XamlingCore.iOS.Unified.Controls.Forms;
using XamlingCore.XamarinThings.Content.Forms;


[assembly: ExportRenderer(typeof(TransitionContentView), typeof(TransitionContentRenderer))]
namespace XamlingCore.iOS.Unified.Controls.Forms
{

    public class TransitionContentRenderer : ViewRenderer<TransitionContentView, UIView>
    {
        private double _duration = .5;
        protected override void OnElementChanged(ElementChangedEventArgs<TransitionContentView> e)
        {
            base.OnElementChanged(e);
            var element = e.NewElement;

            _duration = element.Duration;

            element.SetPreCallback(_onContentChanging);
            element.SetPostCallback(_onContentChanged);
        }

        private async Task _onContentChanging()
        {
            await _fadeOut();
            this.Alpha = 0;

        }

        private async Task _onContentChanged()
        {
            this.Alpha = 0;
            await _fadeIn();
        }


        async Task<bool> _fadeOut()
        {
            var tcs = new TaskCompletionSource<bool>();
            InvokeOnMainThread(() =>
            {
                UIView.Animate(_duration, 0, UIViewAnimationOptions.CurveEaseIn,
                    () =>
                    {
                        this.Alpha = 0;
                    }, () =>
                    {
                        tcs.SetResult(true);
                    }
                    );


            });

            return await tcs.Task;
        }

        async Task<bool> _fadeIn()
        {
            var tcs = new TaskCompletionSource<bool>();
            InvokeOnMainThread(() =>
            {
                UIView.Animate(_duration, 0, UIViewAnimationOptions.CurveEaseOut,
                    () =>
                    {
                        this.Alpha = 1;
                    }, () =>
                    {
                        tcs.SetResult(true);
                    }
                    );


            });

            return await tcs.Task;
        }

    }


}