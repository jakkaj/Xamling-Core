using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using XamlingCore.XamarinThings.Content.Lists;

namespace XamlingCore.Samples.Views.MasterDetailHome.List
{
    public class LongListView : ContentPage
    {
        public LongListView()
        {
            var l = new XXListView();
            l.SetBinding(ListView.ItemsSourceProperty, "Items");
            l.SetBinding(XXListView.NeedMoreDataCommandProperty, "NeedsMoreDataCommand");
            Content = l;
        }
    }
}
