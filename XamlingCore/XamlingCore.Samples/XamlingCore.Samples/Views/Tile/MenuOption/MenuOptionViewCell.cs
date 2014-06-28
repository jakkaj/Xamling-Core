using Xamarin.Forms;

namespace XamlingCore.Samples.Views.Tile.MenuOption
{
    public class MenuOptionView : ContentView
    {
        public MenuOptionView()
        {
            var l = new Label();

            l.TextColor = Color.FromHex("AAAAAA");
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
