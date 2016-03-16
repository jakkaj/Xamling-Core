using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XamlingCore.Portable.View.ViewModel;

namespace XamlingCore.Windows8.Contract
{
    public interface IXPage<T>
        where T:XViewModel
    {
        T ViewModel { get; set; }
    }
}
