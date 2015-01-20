using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using Xamarin.Forms;

namespace XamlingCore.Samples.iOS.NativeViews
{
    public partial class SomeNativeView : UIViewController
    {
        UIScrollView scrollView;
        private UILabel label1;
        public SomeNativeView()
        {
            Title = "Native view";
            ModalTransitionStyle = UIModalTransitionStyle.FlipHorizontal;
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            var frame = new RectangleF(10, 10, 300, 30);
            label1 = new UILabel(frame);
            label1.Text = "New Label";
            View.Add(label1);
        }

        public override void LoadView()
        {
            base.LoadView();
            View.BackgroundColor = UIColor.Gray;
           
           
        }
    }
}