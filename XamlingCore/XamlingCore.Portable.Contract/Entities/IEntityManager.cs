using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XamlingCore.Portable.Contract.EventArgs;
using XamlingCore.Portable.Model.Contract;

namespace XamlingCore.Portable.Contract.Entities
{
    public interface IEntityManager<T> where T : class, IEntity, new()
    {
        Task<List<T>> AllInBucket(string bucket, TimeSpan? maxAge = null);
        Task<bool> IsInBucket(string bucket, T entity);
        Task AddToBucket(string bucket, T entity);
        Task RemoveFromBucket(string bucket, T entity);
        Task ClearAllBuckets();
        Task ClearBucket(string bucket);
        Task PurgeMemory();
        Task<List<T>> Get(List<Guid> ids, TimeSpan? maxAge = null);
        Task<T> Get(Guid id, TimeSpan? maxAge = null);
        Task<List<T>> Set(List<T> entities);
        Task<T> Set(T entity);
        Task Delete(T entity);
        Task MoveToBucket(string bucket, T entity);
        event EventHandler<BucketUpdatedEventArgs> BucketsUpdated;
    }
}