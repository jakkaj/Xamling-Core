using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XamlingCore.Samples.Views.MasterDetailHome.List
{
    public class InPlaceLoaderView : ContentView
    {
        public InPlaceLoaderView()
        {

            var overlay = new AbsoluteLayout();
            var content = new StackLayout();

            content.Children.Add(new Label() {Text = "Loading..."});

            var loadingIndicator = new ActivityIndicator();
            AbsoluteLayout.SetLayoutFlags(content, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(content, new Rectangle(0f, 0f, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
            AbsoluteLayout.SetLayoutFlags(loadingIndicator, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(loadingIndicator, new Rectangle(0.5, 0.5, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
            overlay.Children.Add(content);
            overlay.Children.Add(loadingIndicator);
            loadingIndicator.IsRunning = true;
            Content = overlay;
        }
    }
}
