using System.Collections.Generic;

namespace XamlingCore.Portable.Contract.Services
{
    public interface IContextInfoService
    {
        Dictionary<string, string> Context { get; set; }
        void Add(string key, string value);
    }
}