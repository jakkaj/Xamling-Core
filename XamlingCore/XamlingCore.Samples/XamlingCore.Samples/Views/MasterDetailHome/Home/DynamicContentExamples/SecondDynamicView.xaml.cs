using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using XamlingCore.XamarinThings.Content.Dynamic;

namespace XamlingCore.Samples.Views.MasterDetailHome.Home.DynamicContentExamples
{
    public partial class SecondDynamicView : ContentView, ITransitionView
    {
        public SecondDynamicView()
        {
            Opacity = 0;
            InitializeComponent();
        }

        public int TransitionOut()
        {
            this.FadeTo(0, 1000, Easing.SpringOut);
            return 1000;
        }

        public void TransitionIn()
        {
            this.FadeTo(1, 1000, Easing.BounceIn);
        }
    }
}
