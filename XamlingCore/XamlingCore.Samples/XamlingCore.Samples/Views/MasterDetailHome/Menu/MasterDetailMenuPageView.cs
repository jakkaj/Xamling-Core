using System;
using System.Collections.Generic;
using Xamarin.Forms;
using XamlingCore.XamarinThings.Content.Lists;

namespace XamlingCore.Samples.Views.MasterDetailHome.Menu
{
    public class MasterDetailMenuPageView : ContentPage
    {
        public MasterDetailMenuPageView()
        {
            Icon = "settings.png";
            Title = "menu";
            BackgroundColor = Color.FromHex("FFFFFF");
            var list = new XListView();
            list.BackgroundColor = Color.Transparent;

            list.SetBinding(ListView.ItemsSourceProperty, "Items");
            list.SetBinding(ListView.SelectedItemProperty, "SelectedItem");

            var headerPane = new ContentView();
            headerPane.BackgroundColor = Color.FromHex("333333");
            headerPane.Padding = new Thickness(10, 30, 10, 10);

          
            var stack = new StackLayout();
            stack.Children.Add(headerPane);
           

            stack.Children.Add(list);
            Content = stack;
        }
    }
}
