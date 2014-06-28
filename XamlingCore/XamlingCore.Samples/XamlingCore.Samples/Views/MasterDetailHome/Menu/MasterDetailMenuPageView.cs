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


            var list = new XListView();

            list.SetBinding(ListView.ItemsSourceProperty, "Items");

            Content = list;
        }
    }
}
