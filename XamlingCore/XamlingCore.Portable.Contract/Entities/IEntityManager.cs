using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XamlingCore.Portable.Model.Contract;

namespace XamlingCore.Portable.Contract.Entities
{
    public interface IEntityManager<T> where T : class, IEntity, new()
    {
        Task PurgeMemory();
        Task<List<T>> Get(List<Guid> ids);
        Task<T> Get(Guid id);
        Task<List<T>> Set(List<T> entities);
        Task<T> Set(T entity);
        Task<T> _set(T entity);
    }
}