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
        private uint _time = 750;
        public BaseView()
        {
            Opacity = 0;
            TranslationX = -40;
        }
        public async Task<TimeSpan?> TransitionOut()
        {
            await Task.WhenAll(
                this.FadeTo(0, _time),
                this.TranslateTo(40, 0, _time, Easing.SinOut));
            return null;

        }

        public void TransitionIn()
        {
            this.FadeTo(1, _time);
            this.TranslateTo(0, 0, _time, Easing.SinOut);
        }
    }
}
