﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using XamlingCore.Portable.Contract.Downloaders;
using XamlingCore.Portable.Contract.Entities;
using XamlingCore.Portable.Contract.Repos.Base;
using XamlingCore.Portable.Contract.Serialise;
using XamlingCore.Portable.Model.Response;

namespace XamlingCore.Portable.Data.Repos
{
    public class XWebRepo<TEntity> : IXWebRepo<TEntity>
    {
        private readonly IHttpTransferrer _downloader;
        private readonly IEntitySerialiser _entitySerialiser;

        public XWebRepo(IHttpTransferrer downloader, IEntitySerialiser entitySerialiser)
        {
            _downloader = downloader;
            _entitySerialiser = entitySerialiser;
        }

        public void SetEndPoint(string endPoint)
        {
            Service = endPoint;
        }
       
        public virtual bool OnResultRetrieved(IHttpTransferResult result)
        {
            return true;
        }

        public virtual bool OnEntityRetreived(XResult<TEntity> entity)
        {
            return true;
        }

        public virtual bool OnEntityRetreived<TOverride>(XResult<TOverride> entity)
        {
            return true;
        }

        public virtual bool OnEntityListRetreived(XResult<List<TEntity>> entity)
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

        public async Task<XResult<TEntity>> Upload(byte[] data, string extra, string method)
        {
            var result = await _downloader.Upload(Service + extra, method, data);

            if (result == null || result.Result == null)
            {
                var o = new XResult<TEntity>(null, OperationResults.NoData);

                if (result != null)
                {
                    o.StatusCode = (int)result.HttpStatusCode;
                }
                return o;
            }

            var e = Deserialise<TEntity>(result.Result);
            e.StatusCode = (int) result.HttpStatusCode;

            if (OnEntityRetreived(e))
            {
                return e;
            }

            return null;
        }

        #region POST

        public async Task<XResult<TEntity>> Post<TRequest>(TRequest entity, string extra = null)
        {
            var serialisedData = Serialise(entity);
            return await Post(serialisedData, extra);
        }

        public async Task<XResult<TEntity>> Post(string serialisedData, string extra = null)
        {
            return await _send(serialisedData, extra, "POST");
        }

        public async Task<XResult<List<TEntity>>> PostList(string serialisedData, string extra = null)
        {
            return await _sendList(serialisedData, extra, "POST");
        }

        public async Task<XResult<List<TEntity>>> PostList<TRequest>(TRequest requestEntity, string extra = null, string verb = "POST")
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
        public async Task<XResult<TEntity>> Delete(string extra)
        {
            return await _send(null, extra, "DELETE");
        }
        #endregion

        #region GET

        public async Task<XResult<TOverride>> Get<TOverride>(string extra = null)
        {
            return await _sendOverride<TOverride>(null, extra, "GET");
        }

        public async Task<XResult<TEntity>> Get(string extra = null)
        {
            return await _send(null, extra, "GET");
        }

        public async Task<XResult<TEntity>> Get(Guid id, string extra = null)
        {
            return await _send(null, "/" + id + extra, "GET");
        }

        public async Task<XResult<List<TEntity>>> GetList(string extra = null)
        {
            return await _sendList(null, extra, "GET");
        }

        public async Task<IHttpTransferResult> GetResult(string extra = null)
        {
            return await SendRaw(null, extra, "GET");
        }

        #endregion

        #region PUT

        public async Task<XResult<TEntity>> Put<TRequest>(TRequest entity, string extra = null)
        {
            var serialisedData = Serialise(entity);
            return await Put(serialisedData, extra);
        }

        public async Task<XResult<TEntity>> Put(string serialisedData, string extra = null)
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

        protected XResult<T> Deserialise<T>(string entity)

        {
            try
            {
                if (OverrideSerialiser != null)
                {
                    return OverrideSerialiser.Deserialise<XResult<T>>(entity);
                }
                var e = _entitySerialiser.Deserialise<XResult<T>>(entity);

                if (e == null)
                {
                    return XResult<T>.GetFailed(entity);
                }

                if (e.Message == null && e.Object == null && !e.IsSuccess)
                {
                    e.Message = entity;
                }

                return e;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Des problem: " + ex.Message);
            }

            return XResult<T>.GetFailed(entity);
        }

        protected string Serialise<T>(T entity)
        {
            if (OverrideSerialiser != null)
            {
                return OverrideSerialiser.Serialise(entity);
            }
            return _entitySerialiser.Serialise(entity);
        }

        #endregion

        #region SendGet

        private async Task<XResult<TOverride>> _sendOverride<TOverride>(string serialisedData = null, string extra = null, string method = "POST")
        {
            var result = await SendRaw(serialisedData, extra, method);

            if (result == null || result.Result == null)
            {
                var o = new XResult<TOverride>(null, OperationResults.NoData);
                if (result != null)
                {
                    o.StatusCode = (int) result.HttpStatusCode;
                }
                return o;
            }

            var e = Deserialise<TOverride>(result.Result);
            e.StatusCode = (int)result.HttpStatusCode;
            if (OnEntityRetreived(e))
            {
                return e;
            }

            return null;
        }

        private Task<XResult<TEntity>> _send(string serialisedData = null, string extra = null, string method = "POST")
        {
            return _sendOverride<TEntity>(serialisedData, extra, method);
        }

        private async Task<XResult<List<TEntity>>> _sendList(string serialisedData = null, string extra = null, string method = "POST")
        {
            var result = await SendRaw(serialisedData, extra, method);

            if (result == null || result.Result == null)
            {
                var o = new XResult<List<TEntity>>(null, OperationResults.NoData);

                if (result != null)
                {
                    o.StatusCode = (int) result.HttpStatusCode;
                }

                return o;
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

        protected IEntitySerialiser OverrideSerialiser { get; set; }

        public string Service { get; set; }
    }
}
