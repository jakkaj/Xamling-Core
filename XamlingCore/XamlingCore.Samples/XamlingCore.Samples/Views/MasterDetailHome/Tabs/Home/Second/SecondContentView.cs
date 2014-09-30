using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XamlingCore.Samples.Views.MasterDetailHome.Tabs.Home.Second
{
    public class SecondContentView : ContentView
    {
        public SecondContentView()
        {
            this.SetBinding(Page.TitleProperty, "Title");

            var l = new Label();

            l.TextColor = Color.FromHex("BBBBBB");
            l.SetBinding(Label.TextProperty, "Title");

            var label = new ContentView
            {
                Padding = new Thickness(10, 0, 0, 5),
                Content = l
            };

            var layout = new StackLayout();

            layout.Children.Add(label);

            Content = layout;
        }
    }
}
