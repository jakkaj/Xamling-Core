using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using XamlingCore.Portable.Contract.Entities;
using XamlingCore.Portable.Contract.Repos.Base;
using XamlingCore.Portable.Model.Cache;
using XamlingCore.Portable.Util.Lock;

namespace XamlingCore.Portable.Data.Entities
{
    public class EntityCache : IEntityCache
    {
        private readonly IStorageFileRepo _storageFileRepo;
        private readonly IMemoryCache _cache;


        public EntityCache(IStorageFileRepo storageFileRepo, IMemoryCache cache)
        {
            _storageFileRepo = storageFileRepo;
            _cache = cache;
        }

        public async Task DisableMemoryCache()
        {
            await _cache.Disable();
        }

        public async Task EnableMemoryCache()
        {
            await _cache.Enable();
        }

        private async Task<XCacheItem<T>> _getMemory<T>(string key) where T : class, new()
        {
            return await _cache.Get<T>(key);
        }

        private async Task<XCacheItem<T>> _setMemory<T>(string key, T item, TimeSpan? maxAge) where T : class, new()
        {
            return await _cache.Set(key, item, maxAge);
        }

        public void _deleteMemory<T>(string key) where T : class, new()
        {
            _cache.Delete<T>(key);
        }

        private bool _emptyListFails<T>(T obj, bool allowZeroList) where T : class
        {
            var objt = obj as IList;

            if (objt == null || allowZeroList)
            {
                return false;
            }

            return objt.Count == 0;
        }

        public async Task<T> GetEntity<T>(string key, Func<Task<T>> sourceTask, 
            bool allowExpired = true, bool allowZeroList = true) where T : class, new()
        {
            var locker = XNamedLock.Get(key + "2");//this lock is to cover the gets

            var e = await GetEntity<T>(key);

            if (e != null)
            {
                if (!_emptyListFails(e, allowZeroList))
                {
                    return e;
                }
            }

            T result = null;

            using (var l = await locker.LockAsync())
            {
                //this checks to see if this entity was updated on another lock thread
                e = await GetEntity<T>(key);

                if (e != null)
                {
                    if (!_emptyListFails(e, allowZeroList))
                    {
                        return e;
                    }
                }

                result = await sourceTask();

                if (result != null)
                {
                    _updateItemCacheSource(result, false);
                    await SetEntity<T>(key, result);
                }
            }

            if (result == null && allowExpired)
            {
                return await GetEntity<T>(key);
            }

            return result;
        }

        public async Task<List<T>> GetAll<T>()
            where T : class, new()
        {
            var path = _getDirPath<T>();
            var locker = XNamedLock.Get(path + "getall");

            using (var locked = await locker.LockAsync())
            {
                var f = await _storageFileRepo.GetAll<XCacheItem<T>>(path, false);
                if (f == null)
                {
                    return null;
                }

                var result = new List<T>();
                result.AddRange(f.Select(_ => _.Item));
                return result;
            }
        }

        public async Task<bool> ValidateAge<T>(string key)
            where T : class, new()
        {
            var locker = XNamedLock.Get(key + "3");
            using (var locked = await locker.LockAsync())
            {
                var fullName = _getFullKey<T>(key);

                var f = await _getMemory<T>(key);

                if (f == null)
                {
                    f = await _storageFileRepo.Get<XCacheItem<T>>(fullName);

                    if (f != null && f.Item != null)
                    {
                        _updateItem(f.Item, f);
                        var cacheSet = await _setMemory(key, f.Item, null);
                        _updateItem(cacheSet.Item, cacheSet);
                    }
                }

                if (f == null)
                {
                    return false;
                }

                _updateItemCacheSource(f.Item, true);
                
                if (f.MaxAge == null)
                {
                    return true;
                }

                var ts = DateTime.UtcNow.Subtract(f.DateStamp);

                return ts < f.MaxAge;
            }
        }

        public async Task<TimeSpan?> GetAge<T>(string key) where T : class, new()
        {
            var locker = XNamedLock.Get(key + "3");
            using (var locked = await locker.LockAsync())
            {
                var fullName = _getFullKey<T>(key);

                var f = await _getMemory<T>(key);

                if (f == null)
                {
                    f = await _storageFileRepo.Get<XCacheItem<T>>(fullName);

                    if (f != null && f.Item != null)
                    {
                        _updateItem(f.Item, f);
                        var cacheSet = await _setMemory(key, f.Item, null);
                        _updateItem(cacheSet.Item, cacheSet);
                    }
                }

                if (f == null)
                {
                    return null;
                }

                _updateItemCacheSource(f.Item, true);

                var ts = DateTime.UtcNow.Subtract(f.DateStamp);

                return ts;
            }
        }

        public async Task<T> GetEntity<T>(string key) where T : class, new()
        {
            var fullName = _getFullKey<T>(key);

            var f = await _getMemory<T>(key);

            if (f == null)
            {
                var locker = XNamedLock.Get(key + "3");
                using (var locked = await locker.LockAsync())
                {

                    f = await _storageFileRepo.Get<XCacheItem<T>>(fullName);

                    if (f != null && f.Item != null)
                    {
                        _updateItem(f.Item, f);
                        var cacheEntity = await _setMemory(key, f.Item, 
                            f.MaxAge!=null ? DateTime.UtcNow.Subtract(f.DateStamp.Add(f.MaxAge.Value)) : TimeSpan.MaxValue);
                        _updateItem(cacheEntity.Item, cacheEntity);
                    }

                    if (f == null)
                    {
                        return null;
                    }
                }
            }

            _updateItemCacheSource(f.Item, true);

            if (f.MaxAge == null)
            {
                return f.Item;
            }

            var ts = DateTime.UtcNow.Subtract(f.DateStamp);

            return ts > f.MaxAge ? null : f.Item;
        }

        public async Task<bool> SetEntity<T>(string key, T item) where T : class, new()
        {
            return await SetEntity(key, item, null);
        }

        public async Task<bool> SetEntity<T>(string key, T item, TimeSpan? maxAge) where T : class, new()
        {
            var locker = XNamedLock.Get(key + "setentity");
            using (var locked = await locker.LockAsync())
            {
                var fullName = _getFullKey<T>(key);
                var cacheEntity = await _setMemory(key, item, maxAge);
                _updateItem(cacheEntity.Item, cacheEntity);
                return await _storageFileRepo.Set(cacheEntity, fullName);
            }
        }

        public async Task DeleteAll<T>()
            where T : class, new()
        {
            var path = _getDirPath<T>();

            var locker = XNamedLock.Get(path + "getall");

            using (var locked = await locker.LockAsync())
            {
                var f = await _storageFileRepo.GetAll<XCacheItem<T>>(path, true);
                if (f == null)
                {
                    return;
                }

                foreach (var item in f)
                {
                    await Delete<T>(item.Key);
                }
            }
        }

        public async Task<bool> Delete<T>(string key) where T : class, new()
        {
            var fullName = _getFullKey<T>(key);
            _deleteMemory<T>(key);
            return await _storageFileRepo.Delete(fullName);
        }

        public async Task Clear()
        {
            _cache.Clear();
            await _storageFileRepo.DeleteAll("cache", true);
        }

        void _updateItem<T>(T cacheItem, XCacheItem<T> cacheWrapper)
            where T : class, new()
        {
            var asCache = cacheItem as ICacheInfo;

            if (asCache == null) return;

            asCache.CacheId = cacheWrapper.Id;
            asCache.CacheDateStamp = cacheWrapper.DateStamp;
        }

        void _updateItemCacheSource<T>(T cacheItem, bool fromCache)
            where T : class, new()
        {
            var asCache = cacheItem as ICacheInfo;

            if (asCache == null) return;

            asCache.FromCache = fromCache;
        }


        string _getFullKey<T>(string key)
        {
            var path = Path.Combine(_getDirPath<T>(), string.Format("{0}.cache", key));

            return path;
        }

        string _getDirPath<T>()
        {
            var p = string.Format("cache_{0}", _getTypePath<T>());
            p = Path.Combine("cache", p);

            return p;
        }

        string _getTypePath<T>()
        {
            var t = typeof(T);
            var args = t.GenericTypeArguments;

            string tName = t.Name;

            if (args != null)
            {
                foreach (var a in args)
                {
                    tName += "_" + a.Name;
                }
            }

            tName = tName.Replace("`", "-g-");

            return tName;
        }

        public bool DisableMultitenant
        {
            get { return _storageFileRepo.DisableMultitenant; }
            set { _storageFileRepo.DisableMultitenant = value; }
        }
    }
}
