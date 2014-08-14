using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Xamarin.Forms;

namespace XamlingCore.Samples.iOS.NativeViews
{
    public class SomeNativeView : UIViewController
    {
        UIScrollView scrollView;
        public SomeNativeView()
        {
            Title = "Native view";
            ModalTransitionStyle = UIModalTransitionStyle.FlipHorizontal;
        }
        
        public override void LoadView()
        {
            base.LoadView();
            View.AddSubview(scrollView = new UIScrollView(View.Bounds));
            var totalAmount = new UILabel()
            {
                Text = "$1,000",
                TextColor = UIColor.White,
                TextAlignment = UITextAlignment.Center,
                Font = UIFont.BoldSystemFontOfSize(17),
            };
            scrollView.Add(totalAmount);
        }
    }
}