using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XamlingCore.Portable.Contract.Config
{
    public interface IConfig
    {
        string this[string index] { get; }
    }
}
