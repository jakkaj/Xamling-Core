using System;
using Windows.UI.Core;
using XamlingCore.Portable.Contract.UI;

namespace XamlingCore.UWP.Implementations
{
    public class WindowsUniversalDispatcher : IDispatcher
    {
        public void Invoke(Action action)
        {
            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
            () =>
            {
                action();
            });
        }
    }
}
