using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;

namespace XamlingCore.Portable.Contract.Glue
{
    public interface IGlue
    {
        IContainer Container { get; }
    }
}
