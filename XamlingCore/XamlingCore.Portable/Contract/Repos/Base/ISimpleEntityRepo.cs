using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XamlingCore.Portable.Model.Contract;

namespace XamlingCore.Portable.Contract.Repos.Base
{
    public interface ISimpleEntityRepo<T> where T : class, IEntity, new()
    {
        Task<T> Get(Guid id);
        Task<List<T>> Get();
        Task<bool> Set(T entity);
        Task<bool> Delete(Guid id);

        Task<int> Count();
    }
}
