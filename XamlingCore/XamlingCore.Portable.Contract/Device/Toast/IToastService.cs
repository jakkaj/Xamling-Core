namespace XamlingCore.Portable.Contract.Device.Toast
{
    public interface IToastService
    {
        void SendToast(string title, string content);
    }
}