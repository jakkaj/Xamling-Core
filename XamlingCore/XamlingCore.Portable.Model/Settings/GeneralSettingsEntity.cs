using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XamlingCore.Portable.DTO.Contract;

namespace XamlingCore.Portable.DTO.Settings
{
    public class GeneralSettingsEntity : IEntity
    {
        public string PropertyName { get; set; }
        public string PropertyValue { get; set; }

        public Guid Id { get; set; }
    }
}
