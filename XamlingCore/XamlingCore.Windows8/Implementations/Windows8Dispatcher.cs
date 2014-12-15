using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using XamlingCore.Portable.Contract.UI;

namespace XamlingCore.Windows8.Implementations
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
