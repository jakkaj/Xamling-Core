using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XamlingCore.Portable.Contract.Infrastructure.LocalStorage;
using XamlingCore.Portable.Contract.Repos;
using XamlingCore.Portable.Contract.Repos.Base;
using XamlingCore.Portable.Contract.Serialise;
using XamlingCore.Portable.Model.Settings;
using XamlingCore.Portable.Repos.Base;

namespace XamlingCore.Portable.Repos
{
    public class LocalStorageSettingsRepo : SimpleLocalStorageEntityRepo<GeneralSettingsEntity>, ISettingsRepo
    {
        public LocalStorageSettingsRepo(IApplicationDataHelper applicationDataHelper, 
            ILocalStorageFileRepo localStorageFileRepo) : base(applicationDataHelper, localStorageFileRepo)
        {
        }
    }
}
