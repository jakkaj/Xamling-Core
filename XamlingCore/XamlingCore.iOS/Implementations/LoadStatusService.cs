using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using XamlingCore.Portable.Contract.Services;

namespace XamlingCore.iOS.Implementations
{
    public class LoadStatusService : ILoadStatusService
    {
        public System.Threading.Tasks.Task Loader(System.Threading.Tasks.Task awaiter, string text = null)
        {
            throw new NotImplementedException();
        }

        public System.Threading.Tasks.Task<T> Loader<T>(System.Threading.Tasks.Task<T> awaiter, string text = null)
        {
            throw new NotImplementedException();
        }

        public void PushLoader()
        {
            throw new NotImplementedException();
        }

        public void PopLoader()
        {
            throw new NotImplementedException();
        }

        public bool IsShown
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }
}