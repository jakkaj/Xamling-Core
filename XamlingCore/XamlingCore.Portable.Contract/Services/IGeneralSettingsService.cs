using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XamlingCore.Portable.Contract.Services
{
    public interface IGeneralSettingsService
    {
        Task<string> Get(string settingName);
        Task Set(string settingName, string settingValue);

        Task Delete(string settingName);
        Task ClearAll();

        Task<bool> GetLocationEnabled();
        Task SetLocationEnabled(bool enabled);
    }
}
