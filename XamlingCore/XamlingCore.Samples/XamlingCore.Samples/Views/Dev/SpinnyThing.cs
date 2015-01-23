using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XamlingCore.Samples.Views.Dev
{
    public class SpinnyThing : ContentView
    {
        private bool _stopped;
       
        public void Stop()
        {
            _stopped = true;
        }

        public async void Animate()
        {
            await this.ScaleTo(.5, 0);
            _stopped = false;
            while (!_stopped)
            {
                
                await this.RotateTo(360, 1000, Easing.CubicInOut);
                await this.RotateTo(0, 0);
            }
        }
    }
}
