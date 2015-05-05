using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XamlingCore.Portable.Contract.EventArgs;
using XamlingCore.Portable.Model.Contract;

namespace XamlingCore.Portable.Contract.Entities
{
    public interface IEntityManager<T> : IEntityManager where T : class, IEntity, new()
    {
        Task<List<T>> AllInBucket(string bucket);
        Task<bool> IsInBucket(string bucket, T entity);
        Task AddToBucket(string bucket, T entity);
        Task RemoveFromBucket(string bucket, T entity);
        Task ClearAllBuckets();
        Task ClearBucket(string bucket);
        Task PurgeMemory();
        Task<List<T>> Get(List<Guid> ids);
        Task<T> Get(Guid id);
        Task<List<T>> Set(List<T> entities, TimeSpan? maxAge);
        Task<T> Set(T entity, TimeSpan? maxAge);
        Task Delete(T entity);
        Task MoveToBucket(string bucket, T entity);
        event EventHandler<BucketUpdatedEventArgs> BucketsUpdated;
        Task AddSingleToBucket(string bucket, T entity);
        Task<T> GetSingleFromBucket(string bucket);
        
    }

    public interface IEntityManager
    {
        Task Clear();
        Task ClearAll();
        bool DisableMultitenant { get; set; }
    }
}