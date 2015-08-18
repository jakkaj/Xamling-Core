using System;

namespace XamlingCore.Portable.Contract.Entities
{
    public interface ICacheInfo
    {
        Guid CacheId { get; set; }
        DateTime CacheDateStamp { get; set; }
        bool FromCache { get; set; }
    }
}
