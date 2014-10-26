using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using XamlingCore.iOS.Controls.Native;
using XamlingCore.Samples.iOS.NativeViews.Forms.Playground.Transitions;
using XamlingCore.Samples.Views.Home.Playground.ViewTransitions;

[assembly: ExportRenderer(typeof(ViewTransitionsFormsView), typeof(TransitionsTestViewRender))]
namespace XamlingCore.Samples.iOS.NativeViews.Forms.Playground.Transitions
{
  
    public class TransitionsTestViewRender : XNativeViewRenderer<ViewTransitionsFormsView, TransitionsTestNativeView, TransitionsTestViewModel>
    {
    }
}
