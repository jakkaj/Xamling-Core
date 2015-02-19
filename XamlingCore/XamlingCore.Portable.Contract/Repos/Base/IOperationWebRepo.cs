using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XamlingCore.Portable.Contract.Downloaders;
using XamlingCore.Portable.Model.Response;

namespace XamlingCore.Portable.Contract.Repos.Base
{
    public interface IOperationWebRepo<TEntity>
    {
        Task<OperationResult<TEntity>> Post<TRequest>(TRequest entity, string extra = null);
        Task<OperationResult<TEntity>> Post(string serialisedData, string extra = null);

        Task<bool> Delete(Guid id, string extra = null);
        Task<OperationResult<TEntity>> Get(string extra = null);

        Task<OperationResult<TEntity>> Put<TRequest>(TRequest entity, string extra = null);
        Task<OperationResult<TEntity>> Put(string serialisedData, string extra = null);

        Task<OperationResult<List<TEntity>>> GetList(string extra = null);
        Task<OperationResult<List<TEntity>>> PostList(string serialisedData, string extra = null);
        Task<IHttpTransferResult> PutResult<TRequest>(TRequest entity, string extra = null);
        Task<IHttpTransferResult> PutResult(string serialisedData, string extra = null);
        Task<IHttpTransferResult> PostResult<TRequest>(TRequest entity, string extra = null);
        Task<IHttpTransferResult> PostResult(string serialisedData, string extra = null);
        Task<IHttpTransferResult> GetResult(string extra = null);
        Task<IHttpTransferResult> UploadRaw(byte[] data, string extra, string method);
        Task<OperationResult<TEntity>> Upload(byte[] data, string extra, string method);
        Task<OperationResult<List<TEntity>>> PostList<TRequest>(TRequest requestEntity, string extra = null, string verb = "POST");
    }
}