using System.Collections;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using XamlingCore.XamarinThings.Content.Dynamic;

namespace XamlingCore.XamarinThings.Content.Lists
{
    public class XListView : ListView
    {
        public ICommand MoreDataCommand { get; set; }

        public XListView()
        {
            ItemTemplate = new DataTemplate(typeof(DynamicContentCell));
            this.ItemSelected += XListView_ItemSelected;
            this.ItemAppearing += XXListView_ItemAppearing;
        }

        async void XListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            await Task.Delay(1000);
            SelectedItem = null;
        }

        void XXListView_ItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            var c = ItemsSource as IList;

            if (c == null)
            {
                return;
            }
            if (c[c.Count - 1] == e.Item)
            {
                if (MoreDataCommand != null)
                {
                    MoreDataCommand.Execute(null);
                }
            }
        }

        public static readonly BindableProperty MoreDataCommandProperty =
       BindableProperty.Create<XListView, ICommand>(p => p.MoreDataCommand, null, BindingMode.OneWay, null, _onDataContextChanged);


        private static void _onDataContextChanged(BindableObject obj, ICommand oldValue, ICommand newValue)
        {
            var thisObj = obj as XListView;
            if (thisObj != null) thisObj.MoreDataCommand = newValue;
        }   

        
    }
}
