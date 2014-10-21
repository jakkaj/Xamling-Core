using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using XamlingCore.Portable.Contract.Entities;
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

        private readonly List<T> _memoryCache = new List<T>();

        private readonly AsyncLock _readLock = new AsyncLock();
        private readonly AsyncLock _saveLock = new AsyncLock();

        public EntityManager(IEntityCache entityCache)
        {
            _entityCache = entityCache;            
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

        public async Task<List<T>> Get(List<Guid> ids, TimeSpan? maxAge = null)
        {
            if (ids == null)
            {
                return null;
            }

            var returnList = new List<T>();

            foreach (var id in ids)
            {
                returnList.Add(await _get(id, maxAge));
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
                    return memory;
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

        public async Task<T> _set(T entity)
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

                return memory;
            }
        }

        private string _getKey(Guid id)
        {
            return string.Format("em_{0}", id);
        }
    }
}
