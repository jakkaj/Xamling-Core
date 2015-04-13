using Android.App;
using Android.Content;
using Android.OS;
using Java.Lang;
using System;
using XamlingCore.Portable.Contract.UI;

namespace XamlingCore.Droid.Implementations
{
    

    public class DroidDispatcher : IDispatcher
    {
        /* Alternative method */
        //public readonly Activity owner;
        //public DroidDispatcher(Activity owner)
        //{
        //    this.owner = owner;
        //}
        //public void Invoke(Action action)
        //{
        //    owner.RunOnUiThread(action);
        //}


        public DroidDispatcher()
        {
        }

        public void Invoke(Action action)
        {
            //Another method
            //http://stackoverflow.com/questions/11123621/running-code-in-main-thread-from-another-thread
            var l = Android.OS.Looper.MainLooper; //Previously Context.GetMainLooper()
            Handler mainHandler = new Handler(l);
            Runnable myRunnable = new Runnable(action);
            mainHandler.Post(myRunnable);
        }
    }
}