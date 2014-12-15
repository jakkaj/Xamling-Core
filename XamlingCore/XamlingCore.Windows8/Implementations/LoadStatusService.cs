using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XamlingCore.Portable.Contract.Services;

namespace XamlingCore.Windows8.Implementations
{
    public class LoadStatusService : ILoadStatusService
    {
        public Task Loader(Task awaiter, string text = null)
        {
            throw new NotImplementedException();
        }

        public Task<T> Loader<T>(Task<T> awaiter, string text = null)
        {
            throw new NotImplementedException();
        }

        public void ShowIndicator(string text)
        {
            throw new NotImplementedException();
        }

        public void HideIndicator()
        {
            throw new NotImplementedException();
        }
    }
}
