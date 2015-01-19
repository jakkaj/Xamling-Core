using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using XamlingCore.Portable.View.Properties;

namespace XamlingCore.Samples.Views.MasterDetailHome.List
{
    public class LongListItemView : ContentView
    {
        public LongListItemView()
        {
            var l = new Label();
            l.SetBinding(Label.TextProperty, "Text");

            Content = l;
        }

        protected override void OnBindingContextChanged()
        {
            //var b = BindingContext as LongListItemViewModel;
            //if (b != null)
            //{
            //    Debug.WriteLine("Loaded: " + b.Text);
            //}
            base.OnBindingContextChanged();
        }
    }
}
