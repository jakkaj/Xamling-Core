using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XamlingCore.Portable.Contract.Repos;
using XamlingCore.Portable.Contract.Services;
using XamlingCore.Portable.Messages.Location;
using XamlingCore.Portable.Messages.Settings;
using XamlingCore.Portable.Messages.XamlingMessenger;
using XamlingCore.Portable.Model.Settings;
using XamlingCore.Portable.Util;
using XamlingCore.Portable.Util.Lock;
using XamlingCore.Portable.Util.Util;

namespace XamlingCore.Portable.Service.Settings
{
    public class GeneralSettingsService : IGeneralSettingsService
    {
        private readonly ISettingsRepo _generalSettingsRepo;

        private List<GeneralSettingsEntity> _cache;

        private readonly AsyncLock _lock = new AsyncLock();

        public GeneralSettingsService(ISettingsRepo generalSettingsRepo)
        {
            _generalSettingsRepo = generalSettingsRepo;
        }

        async Task _initCache()
        {
            if (_cache == null)
            {
                _cache = await _generalSettingsRepo.Get();
            }
        }

        async Task<GeneralSettingsEntity> _getCache(string settingName)
        {
            using (var releaser = await _lock.LockAsync())
            {
                if (_cache == null)
                {
                    await _initCache();
                }
                return _cache.FirstOrDefault(_ => _.PropertyName == settingName);
            }
        }

        public async Task<string> Get(string settingName)
        {
            var i = await _getCache(settingName);

            return i != null ? i.PropertyValue : null;
        }

        public async Task Set(string settingName, string settingValue)
        {
            var i = await _getCache(settingName);
            if (i == null)
            {
                i = new GeneralSettingsEntity { Id = Guid.NewGuid(), PropertyName = settingName };
                _cache.Add(i);
            }

            i.PropertyValue = settingValue;
            
            await _generalSettingsRepo.Set(i);
            
            new SettingsUpdatedMessage().Send();
        }

        public async Task Delete(string settingName)
        {
            var i = await _getCache(settingName);
            if (i == null)
            {
                return;
            }

            await _generalSettingsRepo.Delete(i.Id);
        }

        public async Task ClearAll()
        {
            await _initCache();

            foreach (var item in _cache)
            {
                await _generalSettingsRepo.Delete(item.Id);
            }

            _cache.Clear();
        }

        public async Task<bool> GetLocationEnabled()
        {
            var l = await Get("location");

            if (l == null)
            {
                await Set("location", "True");
                return true;
            }
            return l.ToLower() == "true";
        }

        public async Task SetLocationEnabled(bool enabled)
        {
            await Set("location", enabled.ToString());
            new LocationSettingsChangedMessage().Send();
        }
    }
}
