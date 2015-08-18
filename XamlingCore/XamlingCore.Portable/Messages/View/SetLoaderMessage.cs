using XamlingCore.Portable.Messages.XamlingMessenger;

namespace XamlingCore.Portable.Messages.View
{
    public class SetLoaderMessage : XMessage
    {
        public SetLoaderMessage(string text)
        {
            Text = text;
        }

        public string Text { get; }
    }

    public class HideLoaderMessage : XMessage
    {

    }
}
