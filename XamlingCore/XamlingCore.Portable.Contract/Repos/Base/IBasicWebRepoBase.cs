using System.Collections.Generic;
using System.Threading.Tasks;
using XamlingCore.Portable.Contract.Downloaders;

namespace XamlingCore.Portable.Contract.Repos.Base
{
    public interface IBasicWebRepoBase<TEntity> where TEntity : class, new()
    {
        Task<TEntity> Post<TRequest>(TRequest entity, string extra = null);
        Task<TEntity> Post(string serialisedData, string extra = null);

        Task<bool> Delete(string extra = null);
        Task<TEntity> Get(string extra = null);

        Task<TEntity> Put<TRequest>(TRequest entity, string extra = null);
        Task<TEntity> Put(string serialisedData, string extra = null);

        Task<List<TEntity>> GetList(string extra = null);
        Task<List<TEntity>> PostList(string serialisedData, string extra = null);
        Task<IHttpTransferResult> PutResult<TRequest>(TRequest entity, string extra = null);
        Task<IHttpTransferResult> PutResult(string serialisedData, string extra = null);
        Task<IHttpTransferResult> PostResult<TRequest>(TRequest entity, string extra = null);
        Task<IHttpTransferResult> PostResult(string serialisedData, string extra = null);
        Task<IHttpTransferResult> GetResult(string extra = null);
        Task<IHttpTransferResult> UploadRaw(byte[] data, string extra, string method);
        Task<TEntity> Upload(byte[] data, string extra, string method);
        Task<List<TEntity>> PostList<TRequest>(TRequest requestEntity, string extra = null, string verb = "POST");
    }
}