using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using XamlingCore.Portable.Contract.UI;

namespace XamlingCore.iOS.Implementations
{
    public class iOSDispatcher : IDispatcher
    {
        readonly NSObject _owner;

        public iOSDispatcher(NSObject owner)
        {
            _owner = owner;
        }
        public void Invoke(Action action)
        {
            _owner.BeginInvokeOnMainThread(new NSAction(action));
        }
    }


    //public class DispatchAdapter : IDispatchOnUIThread
    //{
    //    public readonly NSObject owner;
    //    public DispatchAdapter(NSObject owner)
    //    {
    //        this.owner = owner;
    //    }
    //    public void Invoke(Action action)
    //    {
    //        owner.BeginInvokeOnMainThread(new NSAction(action));
    //    }
    //}
    //// Android
    //public class DispatchAdapter : IDispatchOnUIThread
    //{
    //    public readonly Activity owner;
    //    public DispatchAdapter(Activity owner)
    //    {
    //        this.owner = owner;
    //    }
    //    public void Invoke(Action action)
    //    {
    //        owner.RunOnUiThread(action);
    //    }
    //}
    //// WP7
    //public class DispatchAdapter : IDispatchOnUIThread
    //{
    //    public void Invoke(Action action)
    //    {
    //        Deployment.Current.Dispatcher.BeginInvoke(action);
    //    }
    //}
}