using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XamlingCore.Portable.Contract.Entities;
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
        private readonly IEntityCache _entityCache;

        private readonly XAsyncLock _lock = new XAsyncLock();

        private const string SETTINGS_KEY = "GeneralSettings";

        public GeneralSettingsService(IEntityCache entityCache)
        {
            _entityCache = entityCache;
        }

        async Task<List<GeneralSettingsEntity>> _getCache()
        {
            //note there is no async lock here, it's to be handled by the caller
            var settings = await _entityCache.GetEntity<List<GeneralSettingsEntity>>(SETTINGS_KEY);

            if (settings == null)
            {
                settings = new List<GeneralSettingsEntity>();
                await _entityCache.SetEntity(SETTINGS_KEY, settings);
            }

            return settings;

        }
        async Task _setCache(List<GeneralSettingsEntity> settings)
        {
            //note there is no async lock here, it's to be handled by the caller
            await _entityCache.SetEntity(SETTINGS_KEY, settings);
        }

        async Task<GeneralSettingsEntity> _getSetting(string settingName)
        {
            var e = await _getCache();
            return e.FirstOrDefault(_ => _.PropertyName == settingName);
        }

        async Task _setValue(string settingName, string value)
        {
            var settings = await _getCache();

            var settingInstance = settings.FirstOrDefault(_ => _.PropertyName == settingName);
            if (settingInstance == null)
            {
                settingInstance = new GeneralSettingsEntity { Id = Guid.NewGuid(), PropertyName = settingName };
                settings.Add(settingInstance);
            }

            settingInstance.PropertyValue = value;

            await _setCache(settings);
        }

        async Task _deleteValue(string settingName)
        {
            var settings = await _getCache();

            var settingInstance = settings.FirstOrDefault(_ => _.PropertyName == settingName);

            if (settingInstance != null)
            {
                settings.Remove(settingInstance);
                await _setCache(settings);
            }
        }

        async Task _clearAll()
        {
            await _entityCache.Delete<List<GeneralSettingsEntity>>(SETTINGS_KEY);
        }

        public async Task<string> Get(string settingName)
        {
            using (var l = await _lock.LockAsync())
            {
                var i = await _getSetting(settingName);
                return i != null ? i.PropertyValue : null;    
            }
        }

        public async Task Set(string settingName, string settingValue)
        {
            using (var l = await _lock.LockAsync())
            {
                await _setValue(settingName, settingValue);
            }
            new SettingsUpdatedMessage().Send();
        }

        public async Task Delete(string settingName)
        {
            using (var l = await _lock.LockAsync())
            {
                await _deleteValue(settingName);
            }
        }

        public async Task ClearAll()
        {
            using (var l = await _lock.LockAsync())
            {
                await _clearAll();
            }
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
