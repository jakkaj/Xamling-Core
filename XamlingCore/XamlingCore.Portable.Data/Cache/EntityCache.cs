using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using XamlingCore.Portable.Contract.Cache;
using XamlingCore.Portable.Contract.Repos.Base;
using XamlingCore.Portable.Model.Cache;
using XamlingCore.Portable.Util;
using XamlingCore.Portable.Util.Lock;

namespace XamlingCore.Portable.Data.Cache
{
    public class EntityCache : IEntityCache
    {
        private readonly ILocalStorageFileRepo _localStorageFileRepo;

        private Dictionary<Type, Dictionary<string, object>> _memoryCache =
            new Dictionary<Type, Dictionary<string, object>>();

        public EntityCache(ILocalStorageFileRepo localStorageFileRepo)
        {
            _localStorageFileRepo = localStorageFileRepo;
        }

        public void DisableMemoryCache()
        {
            if (_memoryCache != null)
            {
                foreach (var item in _memoryCache)
                {
                    if (item.Value != null)
                    {
                        item.Value.Clear();
                    }
                }
            }
            _memoryCache = null;
        }

        public void EnableMemoryCache()
        {
            _memoryCache = new Dictionary<Type, Dictionary<string, object>>();
        }


        private XCacheItem<T> _getMemory<T>(string key) where T : class, new()
        {
            if (_memoryCache == null)
            {
                return null;
            }

            var t = typeof(T);

            if (!_memoryCache.ContainsKey(t)) return null;

            var i = _memoryCache[t];

            if (i == null)
            {
                return null;
            }

            if (!i.ContainsKey(key))
            {
                return null;
            }

            var item = i[key] as XCacheItem<T>;

            return item;
        }

        private XCacheItem<T> _setMemory<T>(string key, T item, DateTime? expireDate) where T : class, new()
        {
            var t = typeof(T);

            Dictionary<string, object> dict = null;

            if (_memoryCache != null)
            {
                if (!_memoryCache.ContainsKey(t))
                {
                    _memoryCache.Add(t, new Dictionary<string, object>());
                }
                dict = _memoryCache[t];
            }
            else
            {
                dict = new Dictionary<string, object>();
            }

            XCacheItem<T> cacheItem = null;

            if (dict.ContainsKey(key))
            {
                cacheItem = dict[key] as XCacheItem<T>;
            }

            if (cacheItem == null)
            {
                cacheItem = new XCacheItem<T>();
                dict[key] = cacheItem;
            }

            if (expireDate != null)
            {
                cacheItem.DateStamp = expireDate.Value;
            }
            else
            {
                cacheItem.DateStamp = DateTime.UtcNow;
            }

            cacheItem.Item = item;

            _updateItem(item, cacheItem);

            return cacheItem;
        }

        public void _deleteMemory<T>(string key) where T : class, new()
        {
            var t = typeof(T);

            if (!_memoryCache.ContainsKey(t)) return;

            var i = _memoryCache[t];

            if (i == null)
            {
                return;
            }

            if (i.ContainsKey(key))
            {
                i.Remove(key);
            }
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

        public async Task<T> GetEntity<T>(string key, Func<Task<T>> sourceTask, TimeSpan? maxAge = null,
            bool allowExpired = true, bool allowZeroList = true) where T : class, new()
        {
            var locker = NamedLock.Get(key + "2");//this lock is to cover the gets

            var e = await GetEntity<T>(key, maxAge);

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
                e = await GetEntity<T>(key, maxAge);
                
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
                return await GetEntity<T>(key, TimeSpan.MaxValue);
            }

            return result;
        }

        public async Task<T> GetEntity<T>(string key, TimeSpan? maxAge = null) where T : class, new()
        {
            var locker = NamedLock.Get(key + "3");
            using (var locked = await locker.LockAsync())
            {
                if (maxAge == null)
                {
                    maxAge = TimeSpan.FromDays(30000);
                }

                var fullName = _getFullKey<T>(key);

                var f = _getMemory<T>(key);

                if (f == null)
                {
                    f = await _localStorageFileRepo.Get<XCacheItem<T>>(fullName);

                    if (f != null && f.Item != null)
                    {
                        _updateItem(f.Item, f);
                        _setMemory(key, f.Item, f.DateStamp);
                    }
                }

                if (f == null)
                {
                    return null;
                }

                _updateItemCacheSource(f.Item, true);

                var ts = DateTime.UtcNow.Subtract(f.DateStamp);

                return ts > maxAge ? null : f.Item;
            }
        }

        public async Task<bool> SetEntity<T>(string key, T item) where T : class, new()
        {
            var locker = NamedLock.Get(key);
            using (var locked = await locker.LockAsync())
            {
                var fullName = _getFullKey<T>(key);
                var cacheEntity = _setMemory(key, item, DateTime.UtcNow);
                return await _localStorageFileRepo.Set(cacheEntity, fullName);
            }
        }

        public async Task<bool> Delete<T>(string key) where T : class, new()
        {
            var fullName = _getFullKey<T>(key);
            _deleteMemory<T>(key);
            return await _localStorageFileRepo.Delete(fullName);
        }

        public async Task Clear()
        {
            _memoryCache.Clear();
            await _localStorageFileRepo.DeleteAll("Cache");
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

            var t = typeof(T);
            var args = t.GenericTypeArguments;

            string tName = t.Name.Replace("`", "-g-");

            if (args != null)
            {
                foreach (var a in args)
                {
                    tName += "_" + a.Name;
                }
            }

            return string.Format("Cache\\{0}\\{1}.cache", tName, key);
        }
    }
}
