using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using XamlingCore.Portable.Contract.Downloaders;
using XamlingCore.Portable.Contract.Repos.Base;
using XamlingCore.Portable.Contract.Serialise;

namespace XamlingCore.Portable.Data.Repos.Base
{
    public abstract class WebRepo<TEntity> : IWebRepo<TEntity> where TEntity : class, new()
    {
        private readonly IHttpTransferrer _downloader;
        private readonly IEntitySerialiser _entitySerialiser;
        protected readonly string Service;

        protected WebRepo(IHttpTransferrer downloader, IEntitySerialiser entitySerialiser, string service)
        {
            _downloader = downloader;
            _entitySerialiser = entitySerialiser;
            Service = service;
        }

        public virtual bool OnResultRetrieved(IHttpTransferResult result)
        {
            return true;
        }

        public virtual bool OnEntityRetreived(TEntity entity)
        {
            return true;
        }

        public virtual bool OnEntityListRetreived(List<TEntity> entity)
        {
            return true;
        }

        public async Task<IHttpTransferResult> UploadRaw(byte[] data, string extra, string method)
        {
            var result = await _downloader.Upload(Service + extra, method, data);
            
            if (!OnResultRetrieved(result))
            {
                return null;
            }

            return result;
        }

        public async Task<TEntity> Upload(byte[] data, string extra, string method)
        {
            var result = await _downloader.Upload(Service + extra, method, data);

            if (result == null || !result.IsSuccessCode || result.Result == null)
            {
                return null;
            }

            var e = Deserialise<TEntity>(result.Result);

            if (OnEntityRetreived(e))
            {
                return e;
            }

            return null;
        }

        #region POST

        public async Task<TEntity> Post<TRequest>(TRequest entity, string extra = null)
        {
            var serialisedData = Serialise(entity);
            return await Post(serialisedData, extra);
        }

        public async Task<TEntity> Post(string serialisedData, string extra = null)
        {
            return await _send(serialisedData, extra, "POST");
        }

        public async Task<List<TEntity>> PostList(string serialisedData, string extra = null)
        {
            return await _sendList(serialisedData, extra, "POST");
        }

        public async Task<List<TEntity>> PostList<TRequest>(TRequest requestEntity, string extra = null, string verb = "POST")
        {
            var serialisedData = Serialise(requestEntity);

            return await _sendList(serialisedData, extra, verb);
        }

        public async Task<IHttpTransferResult> PostResult<TRequest>(TRequest entity, string extra = null)
        {
            var serialisedData = Serialise(entity);
            return await SendRaw(serialisedData, extra);
        }

        public async Task<IHttpTransferResult> PostResult(string serialisedData, string extra = null)
        {
            return await SendRaw(serialisedData, extra, "POST");
        }

        #endregion

        #region DELETE

        public async Task<bool> Delete(Guid id, string extra = null)
        {
            var result = await SendRaw(null, "/" + id + extra, "DELETE");
            return result != null && result.IsSuccessCode;
        }

        #endregion

        #region GET

        public async Task<TEntity> Get(string extra = null)
        {
            return await _send(null, extra, "GET");
        }

        public async Task<TEntity> Get(Guid id, string extra = null)
        {
            return await _send(null, "/" + id + extra, "GET");
        }

        public async Task<List<TEntity>> GetList(string extra = null)
        {
            return await _sendList(null, extra, "GET");
        }

        public async Task<IHttpTransferResult> GetResult(string extra = null)
        {
            return await SendRaw(null, extra, "GET");
        }

        #endregion

        #region PUT

        public async Task<TEntity> Put<TRequest>(TRequest entity, string extra = null)
        {
            var serialisedData = Serialise(entity);
            return await Put(serialisedData, extra);
        }

        public async Task<TEntity> Put(string serialisedData, string extra = null)
        {
            return await _send(serialisedData, extra, "PUT");
        }

        public async Task<IHttpTransferResult> PutResult<TRequest>(TRequest entity, string extra = null)
        {
            var serialisedData = Serialise(entity);
            return await SendRaw(serialisedData, extra, "PUT");
        }

        public async Task<IHttpTransferResult> PutResult(string serialisedData, string extra = null)
        {
            return await SendRaw(serialisedData, extra, "PUT");
        }

        #endregion

        #region Serialisation

        protected T Deserialise<T>(string entity)
            where T : class
        {
            try
            {
                if (OverriseSerialiser != null)
                {
                    return OverriseSerialiser.Deserialise<T>(entity);
                }
                var e = _entitySerialiser.Deserialise<T>(entity);
                return e;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Des problem: " + ex.Message);
            }

            return null;
        }

        protected string Serialise<T>(T entity)
        {
            if (OverriseSerialiser != null)
            {
                return OverriseSerialiser.Serialise(entity);
            }
            return _entitySerialiser.Serialise(entity);
        }

        #endregion

        #region SendGet

        private async Task<TEntity> _send(string serialisedData = null, string extra = null, string method = "POST")
        {
            var result = await SendRaw(serialisedData, extra, method);

            if (result == null || !result.IsSuccessCode || result.Result == null)
            {
                return null;
            }

            var e = Deserialise<TEntity>(result.Result);

            if (OnEntityRetreived(e))
            {
                return e;
            }

            return null;
        }

        private async Task<List<TEntity>> _sendList(string serialisedData = null, string extra = null, string method = "POST")
        {
            var result = await SendRaw(serialisedData, extra, method);

            if (result == null || !result.IsSuccessCode || result.Result == null)
            {
                return null;
            }
           
            var e = Deserialise<List<TEntity>>(result.Result);

            if (OnEntityListRetreived(e))
            {
                return e;
            }

            return null;
        }

        protected async Task<IHttpTransferResult> SendRaw(string serialisedData = null, string extra = null, string method = "POST")
        {
            var result = await _downloader.Download(Service + extra, method, serialisedData);

            if (!OnResultRetrieved(result))
            {
                return null;
            }

            return result;
        }

        #endregion

        protected IEntitySerialiser OverriseSerialiser { get; set; }

    }
}
