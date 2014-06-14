namespace XamlingCore.Portable.Contract.Services
{
    public interface INetworkService
    {
        void RaiseProblemDownloading();
        bool CheckNetworkAvailable();
    }
}