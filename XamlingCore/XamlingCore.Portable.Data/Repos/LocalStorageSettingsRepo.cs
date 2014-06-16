using XamlingCore.Portable.Data.Repos.Base;

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
