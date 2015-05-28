using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace XamlingCore.XamarinThings.Content.Dynamic
{
    public partial class DynamicContentHostView : ContentView
    {
        public DynamicContentHostView()
        {
            InitializeComponent();
        }

        public ContentView ContentOne
        {
            get { return ViewOne; }
        }

        public ContentView ContentTwo
        {
            get { return ViewTwo; }
        }
    }
}
