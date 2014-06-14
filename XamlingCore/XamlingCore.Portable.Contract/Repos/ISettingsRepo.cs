using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XamlingCore.Portable.Contract.Repos.Base;
using XamlingCore.Portable.DTO.Settings;

namespace XamlingCore.Portable.Contract.Repos
{
    public interface ISettingsRepo : ISimpleEntityRepo<GeneralSettingsEntity>
    {
    }
}
