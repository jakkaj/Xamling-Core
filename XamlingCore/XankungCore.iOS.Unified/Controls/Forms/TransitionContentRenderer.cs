using System;
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
        public TransitionContentRenderer()
        {
            
        }

        private double _duration = .5;
        protected override void OnElementChanged(ElementChangedEventArgs<TransitionContentView> e)
        {
            base.OnElementChanged(e);
            
            if (e.OldElement != null)
            {
                e.OldElement.Dispose();
            }

            var element = e.NewElement;

            if (element == null)
            {
                return;
            }

            _duration = element.Duration;

            element.SetPreCallback(_onContentChanging);
            element.SetPostCallback(_onContentChanged);

            
        }

        private async Task _onContentChanging()
        {
            await _fadeOut();
            this.Alpha = new nfloat(.01);

        }

        private async Task _onContentChanged()
        {
            this.Alpha = new nfloat(.01);
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
                        this.Alpha = new nfloat(.01);
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
                        this.Alpha = new nfloat(1);
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