using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using XamlingCore.XamarinThings.Content.Dynamic;

namespace XamlingCore.Samples.Views.MasterDetailHome.Home.DynamicContentExamples
{
    public partial class FirstDynamicView : ContentView, ITransitionView
    {
        public FirstDynamicView()
        {
            this.Opacity = 0;
            InitializeComponent();
        }

        public async Task<TimeSpan?> TransitionOut()
        {
            await this.FadeTo(0, 1000, Easing.SpringOut);
            return null;
        }

        public void TransitionIn()
        {
            this.FadeTo(1, 1000, Easing.BounceIn);
        }
    }
}
