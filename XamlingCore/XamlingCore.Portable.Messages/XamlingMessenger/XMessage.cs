namespace XamlingCore.Portable.Messages.XamlingMessenger
{
    public class XMessage
    {
    }

    public static class XMessageExtension
    {
        public static void Send(this XMessage message)
        {
            XMessenger.Default.Send(message);
        }
    }
}
