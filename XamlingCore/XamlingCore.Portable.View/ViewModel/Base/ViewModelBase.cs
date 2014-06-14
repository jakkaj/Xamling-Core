using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XamlingCore.Portable.Contract.UI;

namespace XamlingCore.Portable.View.ViewModel.Base
{
    public class ViewModelBase : NotifyBase
    {
        public IDispatcher Dispatcher { get; set; }
    }
}
