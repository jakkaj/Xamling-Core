using System;
using XamlingCore.Portable.Model.Cache;

namespace XamlingCore.Portable.Contract.Entities
{
    public interface IMemoryCache
    {
        void Disable();
        void Enable();
        void Delete<T>(string key) where T : class, new();
        void Clear();
        XCacheItem<T> Get<T>(string key) where T : class, new();
        XCacheItem<T> Set<T>(string key, T item, DateTime? expireDate) where T : class, new();
    }
}