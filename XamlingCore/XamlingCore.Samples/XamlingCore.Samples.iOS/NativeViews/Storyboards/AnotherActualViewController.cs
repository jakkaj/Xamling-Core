using System;
using Foundation;
using UIKit;
using System.CodeDom.Compiler;

namespace XamlingCore.Samples.iOS
{
	partial class AnotherActualViewController : UIViewController
	{
		public AnotherActualViewController (IntPtr handle) : base (handle)
		{
		}

	    public override void ViewDidLoad()
	    {
	        base.ViewDidLoad();

            DismissButton.TouchUpInside += DismissButton_TouchUpInside;
	    }

        void DismissButton_TouchUpInside(object sender, EventArgs e)
        {
            DismissViewController(false, null);
        }
	}
}
