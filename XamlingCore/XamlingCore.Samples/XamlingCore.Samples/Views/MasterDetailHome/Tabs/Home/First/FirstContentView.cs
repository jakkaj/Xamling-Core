using Xamarin.Forms;
using XamlingCore.XamarinThings.Contract;

namespace XamlingCore.Samples.Views.MasterDetailHome.Tabs.Home.First
{
    public class FirstContentView : ContentView, IIconView
    {
        public FirstContentView()
        {
            var l = new Label();

            Icon = "settings_vertical.png";

            l.TextColor = Color.FromHex("BBBBBB");
            l.SetBinding(Label.TextProperty, "Title");

            var label = new ContentView
            {
                Padding = new Thickness(10, 0, 0, 5),
                Content = l
            };

            var b = new Button();
            b.Text = "Next page";
            b.SetBinding(Button.CommandProperty, "NextPageCommand");

            var layout = new StackLayout();

            layout.Children.Add(label);
            layout.Children.Add(b);

            Content = layout;
        }

        public FileImageSource Icon { get; set; }
    }
}
