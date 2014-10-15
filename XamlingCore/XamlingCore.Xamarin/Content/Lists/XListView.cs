using Xamarin.Forms;
using XamlingCore.XamarinThings.Content.Dynamic;

namespace XamlingCore.XamarinThings.Content.Lists
{
    public class XListView : ListView
    {
        public XListView()
        {
            ItemTemplate = new DataTemplate(typeof(DynamicContentCell));
        }
    }
}
