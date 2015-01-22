using System;
using System.Collections.Generic;
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
        private Func<Task> _preCallback;

        private Func<Task> _postCallback;

        AsyncLock _locker = new AsyncLock();

        public double Duration { get; set; }

        public TransitionContentView(BindableObject bindingParent)
            : base(bindingParent)
        {
            Duration = .5;
        }

        public void SetPreCallback(Func<Task> callback)
        {
            _preCallback = callback;
        }

        public void SetPostCallback(Func<Task> callback)
        {
            _postCallback = callback;
        }


        protected async override Task<bool> ContentSetOverride(View content)
        {
            while (_preCallback == null)
            {
                await Task.Yield();
            }

            await _preCallback();

            if (!IsVisible)
            {
                IsVisible = true;
            }

            Content = content;

            await _postCallback();

            if (content == null)
            {
                IsVisible = false;
            }



            return true;
        }
    }
}
