using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XamlingCore.Portable.Contract.Downloaders;
using XamlingCore.Portable.Model.Response;

namespace XamlingCore.Portable.Contract.Repos.Base
{
    public interface IXWebRepo<TEntity>
    {
        void SetEndPoint(string endPoint);
        bool OnResultRetrieved(IHttpTransferResult result);
        bool OnEntityRetreived(OperationResult<TEntity> entity);
        bool OnEntityRetreived<TOverride>(OperationResult<TOverride> entity);
        bool OnEntityListRetreived(OperationResult<List<TEntity>> entity);
        Task<IHttpTransferResult> UploadRaw(byte[] data, string extra, string method);
        Task<OperationResult<TEntity>> Upload(byte[] data, string extra, string method);
        Task<OperationResult<TEntity>> Post<TRequest>(TRequest entity, string extra = null);
        Task<OperationResult<TEntity>> Post(string serialisedData, string extra = null);
        Task<OperationResult<List<TEntity>>> PostList(string serialisedData, string extra = null);
        Task<OperationResult<List<TEntity>>> PostList<TRequest>(TRequest requestEntity, string extra = null, string verb = "POST");
        Task<IHttpTransferResult> PostResult<TRequest>(TRequest entity, string extra = null);
        Task<IHttpTransferResult> PostResult(string serialisedData, string extra = null);
        Task<OperationResult<TOverride>> Get<TOverride>(string extra = null);
        Task<OperationResult<TEntity>> Get(string extra = null);
        Task<OperationResult<TEntity>> Get(Guid id, string extra = null);
        Task<OperationResult<List<TEntity>>> GetList(string extra = null);
        Task<IHttpTransferResult> GetResult(string extra = null);
        Task<OperationResult<TEntity>> Put<TRequest>(TRequest entity, string extra = null);
        Task<OperationResult<TEntity>> Put(string serialisedData, string extra = null);
        Task<IHttpTransferResult> PutResult<TRequest>(TRequest entity, string extra = null);
        Task<IHttpTransferResult> PutResult(string serialisedData, string extra = null);
        Task<OperationResult<TEntity>> Delete(string extra);
    }
}