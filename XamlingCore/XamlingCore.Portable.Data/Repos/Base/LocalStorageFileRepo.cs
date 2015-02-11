using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using XamlingCore.Portable.Contract.Infrastructure.LocalStorage;
using XamlingCore.Portable.Contract.Repos.Base;
using XamlingCore.Portable.Contract.Serialise;

namespace XamlingCore.Portable.Data.Repos.Base
{
    public class LocalStorageFileRepo : IStorageFileRepo
    {
        private readonly ILocalStorage _applicationDataHelper;
        private readonly IEntitySerialiser _entitySerialiser;

        public LocalStorageFileRepo(ILocalStorage applicationDataHelper, IEntitySerialiser entitySerialiser)
        {
            _applicationDataHelper = applicationDataHelper;
            _entitySerialiser = entitySerialiser;
        }

        public async Task DeleteAll(string folderName, bool recurse)
        {
            var files = await _applicationDataHelper.GetAllFilesInFolder(folderName, recurse);

            if (files == null)
            {
                return;
            }

            foreach (var item in files)
            {
                await _applicationDataHelper.DeleteFile(item);
            }
        }

        //This repo version is designed to work in a non multi-tenated scenario 
        //(i.e. on a mobile device client with one user)
        public bool DisableMultitenant
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public async Task<bool> Delete(string fileName)
        {
            if (!await _applicationDataHelper.FileExists(fileName)) return true;
            return await _applicationDataHelper.DeleteFile(fileName);
        }

        public async Task<bool> Set<T>(T entity, string fileName)
            where T : class, new()
        {
            var s = _entitySerialiser.Serialise(entity);
            return await _applicationDataHelper.SaveString(fileName, s);
        }

        public async Task<List<T>> GetAll<T>(string folderName, bool recurse)
            where T : class, new()
        {
            var files = await _applicationDataHelper.GetAllFilesInFolder(folderName, recurse);

            if (files == null)
            {
                return null;
            }

            var items = new List<T>();

            foreach (var item in files)
            {
                var i = await Get<T>(Path.Combine(folderName, item));
                
                if (i == null)
                {
                    continue;
                }

                items.Add(i);
            }

            return items;
        }

        public async Task<T> Get<T>(string fileName)
            where T : class, new()
        {
            var d = await _applicationDataHelper.LoadString(fileName);
            return !string.IsNullOrWhiteSpace(d) ? _entitySerialiser.Deserialise<T>(d) : null;

        }

    }
}
