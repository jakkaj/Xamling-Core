using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using XamlingCore.XamarinThings.Content.Dynamic;

namespace XamlingCore.Samples.Views.MasterDetailHome.List
{
    public class XXListView : ListView
    {
        public ICommand NeedMoreDataCommand { get; set; }

        public XXListView()
        {
            ItemTemplate = new DataTemplate(typeof(DynamicContentCell));
            this.ItemSelected += XListView_ItemSelected;


        }

        async void XListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            await Task.Delay(1000);
            SelectedItem = null;
        }

        public static readonly BindableProperty NeedMoreDataCommandProperty =
            BindableProperty.Create<XXListView, ICommand>(p => p.NeedMoreDataCommand, null, BindingMode.OneWay, null, _onDataContextChanged);


        private static void _onDataContextChanged(BindableObject obj, ICommand oldValue, ICommand newValue)
        {
            var thisObj = obj as XXListView;
            if (thisObj != null) thisObj.NeedMoreDataCommand = newValue;
        }        

        protected override void SetupContent(Cell content, int index)
        {
            base.SetupContent(content, index);

            var c = ItemsSource as IList;
            if (c != null)
            {
                var count = c.Count;
                if (index == count - 1 && count == 100)
                {
                    Debug.WriteLine("Loading more");
                    if (NeedMoreDataCommand != null)
                    {
                        NeedMoreDataCommand.Execute(null);
                    }
                }
            }

            var b = content.BindingContext as LongListItemViewModel;
            if (b != null)
            {
                Debug.WriteLine("Loaded Child: " + b.Text);
            }
           
        }

        
    }
}
