using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoreGraphics;
using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using XamlingCore.iOS.Unified.Controls.Loader;
using XamlingCore.Samples.Views.Dev;
[assembly: ExportRenderer(typeof(SpinnyThing), typeof(SpinnyThingRenderer))]
namespace XamlingCore.iOS.Unified.Controls.Loader
{

    public class SpinnyThingRenderer : ViewRenderer<SpinnyThing, UIView>
    {
        public override void WillMoveToSuperview(UIView newsuper)
        {
            base.WillMoveToSuperview(newsuper);

            if (newsuper == null && Element != null)
            {
                Element.Stop();
            }
        }

        protected override void OnElementChanged(ElementChangedEventArgs<SpinnyThing> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                e.OldElement.Stop();
            }
                

            if (e.NewElement!=null)
            {
                e.NewElement.Animate();
            }            
        }

        public override void Draw(CGRect rect)
        {
            base.Draw(rect);

            var cgRect = new CGRect(5, 5, 50, 50);

            using (CGContext g = UIGraphics.GetCurrentContext())
            {
                g.AddEllipseInRect(cgRect);
                g.SetStrokeColor(UIColor.FromRGB(0, 197, 255).CGColor);

                g.SetFillColor(UIColor.White.CGColor);
                g.StrokePath();
                g.FillPath();



            }

            var miniRect = new CGRect(25, 0, 10, 10);

            using (CGContext g = UIGraphics.GetCurrentContext())
            {
                g.AddEllipseInRect(miniRect);
                g.SetFillColor(UIColor.FromRGB(0, 197, 255).CGColor);
                g.FillPath();
            }
        }
    }
}