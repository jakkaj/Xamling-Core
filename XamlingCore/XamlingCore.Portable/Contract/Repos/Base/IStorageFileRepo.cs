using System.Collections.Generic;
using System.Threading.Tasks;

namespace XamlingCore.Portable.Contract.Repos.Base
{
    public interface IStorageFileRepo
    {
        Task<bool> Delete(string fileName);

        Task<bool> Set<T>(T entity, string fileName)
            where T : class, new();

        Task<T> Get<T>(string fileName)
            where T : class, new();

        Task<List<T>> GetAll<T>(string folderName, bool recurse)
            where T : class, new();

        Task DeleteAll(string folderName, bool recurse);

        bool DisableMultitenant { get; set; }
    }
}