using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XamlingCore.Portable.View.ViewModel;

namespace XamlingCore.Windows8.View
{
    public class XViewModelPage<T> : XWindows8Page
        where T:XViewModel
    {
        public T ViewModel { get; set; }
    }
}
