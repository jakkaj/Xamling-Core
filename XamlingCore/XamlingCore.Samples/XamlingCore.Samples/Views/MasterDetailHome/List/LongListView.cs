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
            var l = new XListView();
            l.SetBinding(ListView.ItemsSourceProperty, "Items");
            l.SetBinding(XListView.NeedMoreDataCommandProperty, "NeedsMoreDataCommand");
            Content = l;
        }
    }
}
