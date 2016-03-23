using XamlingCore.Portable.Contract.UI;
using XamlingCore.Portable.Messages.View;
using XamlingCore.Portable.Messages.XamlingMessenger;
using XamlingCore.Portable.View.Special;

namespace XamlingCore.UWP.Implementations
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
