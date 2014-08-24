using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XamlingCore.Portable.Contract.Infrastructure.LocalStorage;
using XamlingCore.Portable.Contract.Repos.Base;
using XamlingCore.Portable.Model.Contract;
using XamlingCore.Portable.Util;
using XamlingCore.Portable.Util.Lock;
using XamlingCore.Portable.Util.Util;

namespace XamlingCore.Portable.Data.Repos.Base
{
    public class SimpleLocalStorageEntityRepo<T> : ISimpleEntityRepo<T> where T : class, IEntity, new()
    {
        private readonly ILocalStorage _applicationDataHelper;
        private readonly ILocalStorageFileRepo _localStorageFileRepo;

        AsyncLock _lock = new AsyncLock();

        private string _name;

        public SimpleLocalStorageEntityRepo(ILocalStorage applicationDataHelper, ILocalStorageFileRepo localStorageFileRepo)
        {
            _applicationDataHelper = applicationDataHelper;
            _localStorageFileRepo = localStorageFileRepo;

            _init();
        }

        async void _init()
        {
            _name = typeof(T).Name;
            _name = _name.Replace("Entity", "");

            using (var l = await _lock.LockAsync())
            {
                var exists = await _applicationDataHelper.EnsureFolderExists(_getFolderName());    
            }
        }

        string _getFolderName()
        {
            return "entity\\" + _name;
        }

        private string _getFile(Guid id)
        {
            return string.Format("entity\\{0}\\{1}", _name, id);
        }

        public async Task<List<T>> Get()
        {
            using (var l = await _lock.LockAsync())
            {
                return await _localStorageFileRepo.GetAll<T>(_getFolderName());
            }
        }

        public async Task<T> Get(Guid id)
        {
            using (var l = await _lock.LockAsync())
            {
                var f = _getFile(id);
                return await _localStorageFileRepo.Get<T>(f);
            }
        }

        public async Task<bool> Set(T entity)
        {
            using (var l = await _lock.LockAsync())
            {
                var f = _getFile(entity.Id);
                return await _localStorageFileRepo.Set(entity, f);
            }
        }

        public async Task<bool> Delete(Guid id)
        {
            using (var l = await _lock.LockAsync())
            {
                var f = _getFile(id);
                return await _localStorageFileRepo.Delete(f);
            }
        }

        public Task<int> Count()
        {
            throw new NotImplementedException();
        }
    }
}
