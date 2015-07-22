using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using XamlingCore.Portable.Contract.Services;
using XamlingCore.Portable.Contract.UI;
using XamlingCore.Portable.Messages.View;
using XamlingCore.Portable.Messages.XamlingMessenger;
using XamlingCore.Portable.View.Special;
using XamlingCore.Windows8.Controls;

namespace XamlingCore.Windows8.Implementations
{
    public class LoadStatusService : LoadStatusServiceBase
    {
        public LoadStatusService(IDispatcher dispatcher)
            : base(dispatcher)
        {
        }

        public override void ShowIndicator(string text)
        {
           new SetLoaderMessage(text).Send();
        }

        public override void HideIndicator()
        {
            new HideLoaderMessage().Send();
        }
    }
}
