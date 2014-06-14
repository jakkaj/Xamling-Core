using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XamlingCore.Portable.View.ViewModel.Base
{
    public class ViewModelBase : NotifyBase
    {
        public Action<Action> Dispatcher { get; set; }
    }
}
