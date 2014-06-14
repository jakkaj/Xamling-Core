using System.Threading.Tasks;

namespace XamlingCore.Portable.Contract.Downloaders
{
    public interface IDownloader
    {
        Task<IDownloadResult> Download(string url, string verb = "GET", string data = null);
        Task<IDownloadResult> Upload(string url, string verb = "GET", byte[] data = null);
    }
}