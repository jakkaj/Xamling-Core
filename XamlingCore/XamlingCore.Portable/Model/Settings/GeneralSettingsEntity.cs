using System;
using XamlingCore.Portable.Model.Contract;

namespace XamlingCore.Portable.Model.Settings
{
    public class GeneralSettingsEntity : IEntity
    {
        public string PropertyName { get; set; }
        public string PropertyValue { get; set; }

        public Guid Id { get; set; }
    }
}
