using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XamlingCore.Portable.Model.Contract;

namespace XamlingCore.Portable.Contract.Entities
{
    public interface IEntityBucket<T> where T : class, IEntity, new()
    {
        Task<List<Guid>> AllInBucket(string bucket);
        Task<bool> IsInBucket(string bucket, Guid guid);
        Task AddToBucket(string bucket, Guid guid);
        Task RemoveFromBucket(string bucket, Guid guid);
        Task ClearAll();
        Task ClearBucket(string bucket);
        Task MoveToBucket(string bucket, Guid guid);
    }
}