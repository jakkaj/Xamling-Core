using System;
using System.Threading.Tasks;
using XamlingCore.Portable.Model.Cache;

namespace XamlingCore.Portable.Contract.Entities
{
    public interface IMemoryCache
    {
        Task Disable();
        Task Enable();
        Task<bool> Delete<T>(string key) where T : class, new();
        Task Clear();
        Task<XCacheItem<T>> Get<T>(string key) where T : class, new();
        Task<XCacheItem<T>> Set<T>(string key, T item, TimeSpan? maxAge) where T : class, new();
    }
}