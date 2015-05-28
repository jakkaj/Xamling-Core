using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using XamlingCore.XamarinThings.Content.Dynamic;

namespace XamlingCore.Samples.Views.MasterDetailHome.Transitions
{
    public class BaseView : ContentView, ITransitionView
    {
        public BaseView()
        {
            Opacity = 0;
        }
        public async Task TransitionOut()
        {
            await this.FadeTo(0, 500);
        }

        public void TransitionIn()
        {
            this.FadeTo(1, 500);
        }
    }
}
