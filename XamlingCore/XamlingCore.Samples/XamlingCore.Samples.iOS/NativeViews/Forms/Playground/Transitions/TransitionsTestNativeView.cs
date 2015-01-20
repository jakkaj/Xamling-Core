using System.Drawing;
using UIKit;
using XamlingCore.iOS.Unified.Controls.Native;
using XamlingCore.Samples.Views.Home.Playground.ViewTransitions;

namespace XamlingCore.Samples.iOS.NativeViews.Forms.Playground.Transitions
{
    public class TransitionsTestNativeView : XNativeView<TransitionsTestViewModel>
    {
        private UIButton _testButton;


        protected override void OnInitialise()
        {
            _testButton = UIButton.FromType(UIButtonType.RoundedRect);
            _testButton.Frame = new RectangleF(new PointF(15, 20), new SizeF(125, 125));
            _testButton.SetTitle("Some Button", UIControlState.Normal);
            
            AddSubview(_testButton);
        }
    }
}
