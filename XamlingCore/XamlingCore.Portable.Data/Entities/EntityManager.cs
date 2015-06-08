using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        //private readonly XAsyncLock _readLock = new XAsyncLock();
        private readonly XAsyncLock _saveLock = new XAsyncLock();

        public event EventHandler<BucketUpdatedEventArgs> BucketsUpdated;

        private static List<IEntityManager> _allManagers = new List<IEntityManager>();

        public EntityManager(IEntityCache entityCache, IEntityBucket<T> bucket)
        {
            _allManagers.Add(this);
            _entityCache = entityCache;
            _bucket = bucket;

            _bucket.BucketsUpdated += _bucket_BucketUpdated;
        }

        private void _bucket_BucketUpdated(object sender, EventArgs e)
        {
            if (BucketsUpdated != null)
            {
                BucketsUpdated(this, e as BucketUpdatedEventArgs);
            }
        }

        public async Task<List<T>> AllInBucket(string bucket)
        {
            var i = await _bucket.AllInBucket(bucket);

            if (i == null)
            {
                return null;
            }

            var list = await Get(i);

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

        public async Task AddSingleToBucket(string bucket, T entity)
        {
            if (entity == null)
            {
                return;
            }

            await _bucket.AddSingleToBucket(bucket, entity.Id);
        }

        public async Task<T> GetSingleFromBucket(string bucket)
        {
            var g = await _bucket.GetSingleFromBucket(bucket);
            
            if (g == Guid.Empty)
            {
                return null;
            }

            return await Get(g);
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
            using (var l2 = await _saveLock.LockAsync())
            {
                _memoryCache.Clear();
            }
        }

        private async Task _reconcileBucket(string bucket, IEnumerable<Guid> ids, List<T> entities)
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

        public async Task<List<T>> Get(List<Guid> ids, Func<Task<T>> sourceTask, TimeSpan? maxAge = null)
        {
            if (ids == null)
            {
                return null;
            }

            var returnList = new List<T>();
            var idCopy = ids.ToList();//copy it in case it's altered by something else during this iteration
            foreach (var id in idCopy)
            {
                var e = await _get(id, sourceTask, maxAge);
                if (e != null)
                {
                    returnList.Add(e);
                }
            }

            return returnList;
        }

        public async Task<List<T>> Get(List<Guid> ids)
        {
            return await Get(ids, null, null);
        }

        public async Task<T> Get(Guid id)
        {
            return await _get(id, null, null);
        }

        private async Task<T> _get(Guid id, Func<Task<T>> sourceTask, TimeSpan? maxAge)
        {
            var memory = _memoryCache.FirstOrDefault(_ => _.Id == id);

            if (memory != null)
            {
                if(await _entityCache.ValidateAge<T>(_getKey(id)))
                {
                    return memory;
                }
            }

            using (var lRead = await XNamedLock.Get("entm_" + id).LockAsync())
            {
                T cache = null;

                if (sourceTask != null)
                {
                    cache = await _entityCache.GetEntity<T>(_getKey(id), sourceTask);
                }
                else
                {
                    cache = await _entityCache.GetEntity<T>(_getKey(id));
                }

                if (cache == null)
                {
                    return null;
                }

                var cResult = await Set(cache, maxAge);

                return cResult;
            }

        }

        public async Task<List<T>> Set(List<T> entities, TimeSpan? maxAge)
        {
            if (entities == null)
            {
                return null;
            }

            var returnList = new List<T>();

            foreach (var item in entities)
            {
                var savedItem = await Set(item, maxAge);
                returnList.Add(savedItem);
            }

            return returnList;
        }

        public async Task<T> Set(T entity, TimeSpan? maxAge)
        {
            return await _set(entity, maxAge);
        }

        private async Task<T> _set(T entity, TimeSpan? maxAge)
        {
            if (entity == null)
            {
                return null;
            }
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
                    entity.CopyProperties(memory);
                }

                await _entityCache.SetEntity(_getKey(entity.Id), memory, maxAge);

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

            using (var lRead = await XNamedLock.Get("entm_" + entity.Id).LockAsync())
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

        public async Task Clear()
        {
            using (var lSave = await _saveLock.LockAsync())
            {
                _memoryCache.Clear();
            }
        }

        public async Task ClearAll()
        {
            foreach (var i in _allManagers)
            {
                await i.Clear();
            }
        }
    }


}
