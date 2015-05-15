using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XamlingCore.Portable.Contract.Downloaders;
using XamlingCore.Portable.Model.Response;

namespace XamlingCore.Portable.Contract.Repos.Base
{
    public interface IOperationWebRepo<TEntity>
    {
        Task<XResult<TEntity>> Post<TRequest>(TRequest entity, string extra = null);
        Task<XResult<TEntity>> Post(string serialisedData, string extra = null);

        Task<bool> Delete(Guid id, string extra = null);
        Task<XResult<TEntity>> Get(string extra = null);

        Task<XResult<TEntity>> Put<TRequest>(TRequest entity, string extra = null);
        Task<XResult<TEntity>> Put(string serialisedData, string extra = null);

        Task<XResult<List<TEntity>>> GetList(string extra = null);
        Task<XResult<List<TEntity>>> PostList(string serialisedData, string extra = null);
        Task<IHttpTransferResult> PutResult<TRequest>(TRequest entity, string extra = null);
        Task<IHttpTransferResult> PutResult(string serialisedData, string extra = null);
        Task<IHttpTransferResult> PostResult<TRequest>(TRequest entity, string extra = null);
        Task<IHttpTransferResult> PostResult(string serialisedData, string extra = null);
        Task<IHttpTransferResult> GetResult(string extra = null);
        Task<IHttpTransferResult> UploadRaw(byte[] data, string extra, string method);
        Task<XResult<TEntity>> Upload(byte[] data, string extra, string method);
        Task<XResult<List<TEntity>>> PostList<TRequest>(TRequest requestEntity, string extra = null, string verb = "POST");
    }
}