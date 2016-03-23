using System;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using XamlingCore.Portable.Contract.UI;

namespace XamlingCore.UWP.Implementations
{
    public class Windows8Dispatcher : IDispatcher
    {
        public void Invoke(Action action)
        {
            CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                () =>
                {
                    action();
                });
        }
    }
}
