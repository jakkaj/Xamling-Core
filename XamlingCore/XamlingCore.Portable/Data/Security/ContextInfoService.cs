using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XamlingCore.Portable.Contract.Services;

namespace XamlingCore.Portable.Data.Security
{
    public class ContextInfoService : IContextInfoService
    {
        public Dictionary<string, string> Context { get; set; }

        public ContextInfoService()
        {
            Context = new Dictionary<string, string>();
        }
        
        public void Add(string key, string value)
        {
            if (Context.ContainsKey(key))
            {
                Context[key] = value;
            }
            else
            {
                Context.Add(key, value);
            }
        }
    }
}
