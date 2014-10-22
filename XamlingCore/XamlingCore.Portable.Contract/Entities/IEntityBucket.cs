using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace XamlingCore.Portable.Contract.Entities
{
    public interface IEntityBucket
    {
        Task<List<Guid>> AllInBucket(string bucket);
        Task<bool> IsInBucket(string bucket, Guid guid);
        Task AddToBucket(string bucket, Guid guid);
        Task RemoveFromBucket(string bucket, Guid guid);
        Task ClearAll();
        Task ClearBucket(string bucket);
    }
}