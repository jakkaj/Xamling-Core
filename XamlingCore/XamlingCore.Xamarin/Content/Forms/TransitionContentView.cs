using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using XamlingCore.XamarinThings.Content.Dynamic;

namespace XamlingCore.XamarinThings.Content.Forms
{


    public class TransitionContentView : DynamicContentView
    {
        public int Duration { get; set; }

        public TransitionContentView(BindableObject obj)
            :base(obj)
        {
            Duration = 1000;
        }

        public TransitionContentView()
        {
            Duration = 1000;
        }
        
        protected async override Task<bool> ContentSetOverride(View content)
        {
            if (!IsVisible)
            {
                IsVisible = true;
            }

            await this.FadeTo(0, Convert.ToUInt32(Duration), Easing.SinIn);

            try
            {
                Content = content;
            }
            catch
            {
                
            }

            

            if (Content == null)
            {
                IsVisible = false;
            }
            else
            {
                await this.FadeTo(1, Convert.ToUInt32(Duration), Easing.SinOut);
            }

            return true;
        }

    }
}
