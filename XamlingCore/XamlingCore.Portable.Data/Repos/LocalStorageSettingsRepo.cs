using XamlingCore.Portable.Contract.Infrastructure.LocalStorage;
using XamlingCore.Portable.Contract.Repos;
using XamlingCore.Portable.Contract.Repos.Base;
using XamlingCore.Portable.Data.Repos.Base;
using XamlingCore.Portable.Model.Settings;

namespace XamlingCore.Portable.Data.Repos
{
    public class LocalStorageSettingsRepo : SimpleLocalStorageEntityRepo<GeneralSettingsEntity>, ISettingsRepo
    {
        public LocalStorageSettingsRepo(ILocalStorage applicationDataHelper, 
            ILocalStorageFileRepo localStorageFileRepo) : base(applicationDataHelper, localStorageFileRepo)
        {
        }
    }
}
