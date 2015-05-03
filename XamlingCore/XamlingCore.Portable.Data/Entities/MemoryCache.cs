using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XamlingCore.Portable.Contract.Entities;
using XamlingCore.Portable.Model.Cache;

namespace XamlingCore.Portable.Data.Entities
{
    public class MemoryCache : IMemoryCache
    {
        private Dictionary<Type, Dictionary<string, object>> _memoryCache =
            new Dictionary<Type, Dictionary<string, object>>();

        public void Clear()
        {
            _memoryCache.Clear();
        }

        public void Disable()
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

        public void Enable()
        {
            _memoryCache = new Dictionary<Type, Dictionary<string, object>>();
        }

        public XCacheItem<T> Get<T>(string key) where T : class, new()
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

        public XCacheItem<T> Set<T>(string key, T item, DateTime? expireDate) where T : class, new()
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
            cacheItem.Key = key;

            return cacheItem;
        }

        public void Delete<T>(string key) where T : class, new()
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


    }
}
