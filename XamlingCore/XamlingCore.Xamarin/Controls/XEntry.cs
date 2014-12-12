using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XamlingCore.XamarinThings.Controls
{
    public class XEntry : Entry
    {
        public string TextBind
        {
            set
            {
                SetBinding(TextProperty, new Binding(value, BindingMode.TwoWay));
            }
        }
    }
}
