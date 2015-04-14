using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using XamlingCore.Portable.Contract.UI;

namespace XamlingCore.Windows8.Implementations
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
