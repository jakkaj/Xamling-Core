using System.Threading.Tasks;

namespace XamlingCore.Portable.Contract.Downloaders
{
    public interface ISimpleDownloader
    {
        Task<byte[]> DownloadBytes(string url);
        Task<bool> UploadBytes(string url, byte[] data);
    }
}