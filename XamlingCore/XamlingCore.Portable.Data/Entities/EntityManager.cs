using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using XamlingCore.Portable.Contract.Entities;
using XamlingCore.Portable.Contract.EventArgs;
using XamlingCore.Portable.Messages.Entities;
using XamlingCore.Portable.Messages.XamlingMessenger;
using XamlingCore.Portable.Model.Contract;
using XamlingCore.Portable.Util.Lock;

namespace XamlingCore.Portable.Data.Entities
{
    /// <summary>
    /// This class stores instances of entities and ensures you get and update the same instance
    /// The entity cache on the other hand does not care about instances, and servers as the underlying datastore for those types
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EntityManager<T> : IEntityManager<T> where T : class, IEntity, new()
    {
        private readonly IEntityCache _entityCache;
        private readonly IEntityBucket<T> _bucket;

        private readonly List<T> _memoryCache = new List<T>();

        private readonly AsyncLock _readLock = new AsyncLock();
        private readonly AsyncLock _saveLock = new AsyncLock();

        public event EventHandler<BucketUpdatedEventArgs> BucketsUpdated;

        public EntityManager(IEntityCache entityCache, IEntityBucket<T> bucket)
        {
            _entityCache = entityCache;
            _bucket = bucket;

            _bucket.BucketsUpdated += _bucket_BucketUpdated;
        }

        void _bucket_BucketUpdated(object sender, EventArgs e)
        {
            if (BucketsUpdated != null)
            {
                BucketsUpdated(this, e as BucketUpdatedEventArgs);
            }
        }        

        public async Task<List<T>> AllInBucket(string bucket, TimeSpan? maxAge = null)
        {
            var i = await _bucket.AllInBucket(bucket);
            
            if (i == null)
            {
                return null;
            }

            var list = await Get(i, maxAge);
            
            await _reconcileBucket(bucket, i, list);
            
            return list;
        }

        public async Task MoveToBucket(string bucket, T entity)
        {
            if (entity == null)
            {
                return;
            }

            await _bucket.MoveToBucket(bucket, entity.Id);
        }

        public async Task<bool> IsInBucket(string bucket, T entity)
        {
            if (entity == null)
            {
                return false;
            }

            return await _bucket.IsInBucket(bucket, entity.Id);
        }

        public async Task AddToBucket(string bucket, T entity)
        {
            if (entity == null)
            {
                return;
            }

            await _bucket.AddToBucket(bucket, entity.Id);
        }

        public async Task RemoveFromBucket(string bucket, T entity)
        {
            if (entity == null)
            {
                return;
            }

            await _bucket.RemoveFromBucket(bucket, entity.Id);
        }

        public async Task ClearAllBuckets()
        {
            await _bucket.ClearAll();
        }

        public async Task ClearBucket(string bucket)
        {
            await _bucket.ClearBucket(bucket);
        }

        public async Task PurgeMemory()
        {
            using (var l1 = await _readLock.LockAsync())
            {
                using (var l2 = await _saveLock.LockAsync())
                {
                    _memoryCache.Clear();
                }
            }
        }

        async Task _reconcileBucket(string bucket, IEnumerable<Guid> ids, List<T> entities)
        {
            if (ids == null || entities == null)
            {
                return;
            }

            var removes = ids.Where(i => entities.FirstOrDefault(_ => _.Id == i) == null).ToList();

            foreach (var i in removes)
            {
                await _bucket.RemoveFromBucket(bucket, i);    
            }
        }

        public async Task<List<T>> Get(List<Guid> ids, TimeSpan? maxAge = null)
        {
            if (ids == null)
            {
                return null;
            }

            var returnList = new List<T>();

            foreach (var id in ids)
            {
                var e = await _get(id, maxAge);
                if (e != null)
                {
                    returnList.Add(e);    
                }
            }

            return returnList;
        }

        public async Task<T> Get(Guid id, TimeSpan? maxAge = null)
        {
            return await _get(id, maxAge);
        }

        private async Task<T> _get(Guid id, TimeSpan? maxAge)
        {
            using (var lRead = await _readLock.LockAsync())
            {
                var memory = _memoryCache.FirstOrDefault(_ => _.Id == id);

                if (memory != null)
                {
                    if (maxAge != null)
                    {
                        var age = await _entityCache.GetAge<T>(_getKey(id));
                        if (age < maxAge)
                        {
                            return memory;
                        }
                    }
                    else
                    {
                        return memory;
                    }
                }

                var cache = await _entityCache.GetEntity<T>(_getKey(id), maxAge);

                if (cache == null)
                {
                    return null;
                }

                var cResult = await Set(cache);

                return cResult;
            }

        }

        public async Task<List<T>> Set(List<T> entities)
        {
            if (entities == null)
            {
                return null;
            }

            var returnList = new List<T>();

            foreach (var item in entities)
            {
                var savedItem = await Set(item);
                returnList.Add(savedItem);
            }

            return returnList;
        } 

        public async Task<T> Set(T entity)
        {
            return await _set(entity);
        }

        private async Task<T> _set(T entity)
        {
            using (var lSave = await _saveLock.LockAsync())
            {
                var memory = _memoryCache.FirstOrDefault(_ => _.Id == entity.Id);

                //we do have this object type in memory

                if (memory == null)
                {
                    _memoryCache.Add(entity);
                    memory = entity;
                }

                if (!ReferenceEquals(memory, entity))
                {
                    //update the in memory version, save it to cache, return in memory version
                    Mapper.Map(entity, memory);
                }

                await _entityCache.SetEntity(_getKey(entity.Id), memory);

                new EntityUpdatedMessage<T>(entity).Send();

                return memory;
            }
        }

        public async Task Delete(T entity)
        {
            var e = await Get(entity.Id);

            if (e == null)
            {
                return;
            }

            using (var lRead = await _readLock.LockAsync())
            {
                using (var lSave = await _saveLock.LockAsync())
                {
                    if (_memoryCache.Contains(e))
                    {
                        _memoryCache.Remove(e);    
                    }

                    await _entityCache.Delete<T>(_getKey(e.Id));
                }
            }
        }

        private string _getKey(Guid id)
        {
            return string.Format("em_{0}", id);
        }
    }
}
