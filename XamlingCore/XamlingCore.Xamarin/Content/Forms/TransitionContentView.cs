using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Xamarin.Forms;
using XamlingCore.Portable.Data.Glue;
using XamlingCore.Portable.Util.Lock;
using XamlingCore.XamarinThings.Content.Dynamic;
using XamlingCore.XamarinThings.Contract;

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
            await this.FadeTo(0, Convert.ToUInt32(Duration), Easing.SinIn);

            if (!IsVisible)
            {
                IsVisible = true;
            }

            Content = content;

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
