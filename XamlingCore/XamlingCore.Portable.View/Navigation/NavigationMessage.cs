using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XamlingCore.Portable.Messages.XamlingMessenger;

namespace XamlingCore.Portable.View.Navigation
{
    public class NavigationMessage : XMessage
    {
        public NavigationMessage(object content, bool isBack)
        {
            Content = content;
            IsBack = isBack;
        }

        public object Content { get; }

        public bool IsBack { get; }
    }
}
