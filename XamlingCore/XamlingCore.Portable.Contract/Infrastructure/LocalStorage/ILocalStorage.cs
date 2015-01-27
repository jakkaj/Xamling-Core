using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace XamlingCore.Portable.Contract.Infrastructure.LocalStorage
{
    public interface ILocalStorage
    {
        Task<bool> FileExists(string fileName);
        Task<bool> Copy(string source, string newName, bool replace = true);

        Task<byte[]> Load(string fileName);
        Task<Stream> LoadStream(string fileName);
        Task<string> LoadString(string fileName);

        Task Save(string fileName, byte[] data);
        Task SaveStream(string fileName, Stream stream);
        Task<bool> SaveString(string fileName, string data);

        Task<bool> DeleteFile(string fileName);
        
        Task<List<string>> GetAllFilesInFolder(string folderPath, bool recurse);
        Task<bool> IsZero(string fileName);
        string GetFullPath(string fileName);
    }
}