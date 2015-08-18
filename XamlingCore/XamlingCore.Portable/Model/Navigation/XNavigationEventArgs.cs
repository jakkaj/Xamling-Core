using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XamlingCore.Portable.Model.Navigation
{
    public class XNavigationEventArgs : EventArgs
    {
        private readonly NavigationDirection _direction;

        public XNavigationEventArgs(NavigationDirection direction)
        {
            _direction = direction;
        }

        public NavigationDirection Direction
        {
            get { return _direction; }
        }
    }
}
