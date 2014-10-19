using System.Threading.Tasks;
using Xamarin.Forms;
using XamlingCore.XamarinThings.Content.Dynamic;

namespace XamlingCore.XamarinThings.Content.Lists
{
    public class XListView : ListView
    {
        public XListView()
        {
            ItemTemplate = new DataTemplate(typeof(DynamicContentCell));
            this.ItemSelected += XListView_ItemSelected;
        }

        async void XListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            await Task.Delay(1000);
            SelectedItem = null;
        }

        
    }
}
