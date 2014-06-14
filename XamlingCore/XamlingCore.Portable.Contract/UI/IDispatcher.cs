using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XamlingCore.Portable.Contract.UI
{
    public interface IDispatcher
    {
        void Invoke(Action action);
    }
    
}
