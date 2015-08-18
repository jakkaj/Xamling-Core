using System.Threading.Tasks;

namespace XamlingCore.Portable.Contract.Downloaders
{
    public interface IHttpTransferrer
    {
        Task<IHttpTransferResult> Download(string url, string verb = "GET", string data = null);
        Task<IHttpTransferResult> Upload(string url, string verb = "GET", byte[] data = null);
    }
}