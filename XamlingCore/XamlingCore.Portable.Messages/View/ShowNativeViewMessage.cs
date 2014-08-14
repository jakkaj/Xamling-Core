using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XamlingCore.Portable.Messages.XamlingMessenger;

namespace XamlingCore.Portable.Messages.View
{
    public class ShowNativeViewMessage : XMessage
    {
        private readonly string _viewName;

        public ShowNativeViewMessage(string viewName)
        {
            _viewName = viewName;
        }

        public string ViewName
        {
            get { return _viewName; }
        }
    }
}
