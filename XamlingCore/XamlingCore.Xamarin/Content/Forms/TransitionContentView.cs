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
    public class TransitionContentView : DynamicContentView, IDisposable
    {
        private Func<Task> _preCallback;

        private Func<Task> _postCallback;

        AsyncLock _locker = new AsyncLock();

        public double Duration { get; set; }

        private bool _isDisposed;

        public TransitionContentView(BindableObject bindingParent)
            : base(bindingParent)
        {
            Duration = .5;
        }

        public TransitionContentView()
        {

        }

        public void ClearCallbacks()
        {
            _preCallback = null;
            _postCallback = null;
        }

        public void SetPreCallback(Func<Task> callback)
        {
            _preCallback = callback;
            _isDisposed = false;
        }

        public void SetPostCallback(Func<Task> callback)
        {
            _postCallback = callback;
            _isDisposed = false;
        }



        protected async override Task<bool> ContentSetOverride(View content)
        {
            Debug.WriteLine("1");
            
                if (_isDisposed)
                {
                    return true;
                }
                Debug.WriteLine("2");
                while (_preCallback == null)
                {
                    await Task.Yield();
                }
                Debug.WriteLine("3");
                if (_isDisposed)
                {
                    return true;
                }
                Debug.WriteLine("4");
                await _preCallback();
                if (_isDisposed)
                {
                    return true;
                }
                if (!IsVisible)
                {
                    IsVisible = true;
                }
                Debug.WriteLine("5");
                Content = content; //this is hacky, but it's causing issues. 

                await _postCallback();
                Debug.WriteLine("6");
                if (_isDisposed)
                {
                    return true;
                }
                Debug.WriteLine("7");
                if (content == null)
                {
                    IsVisible = false;
                }
            
            
            return true;
        }



        public void Dispose()
        {
            ClearCallbacks();
            _isDisposed = true;
        }
    }
}
