using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XamlingCore.Portable.Data.Entities
{
    public class KeyEntityBase
    {
        public virtual string GetKey(string key)
        {
            return key;
        }
    }
}
