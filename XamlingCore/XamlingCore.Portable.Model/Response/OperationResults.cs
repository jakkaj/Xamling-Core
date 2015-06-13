using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XamlingCore.Portable.Model.Response
{
    [Flags]
    public enum OperationResults
    {
        NotSet=0,
        Success = 1,
        Failed = 2,
        NotFound = 4,
        NotAuthorised = 8,
        BadRequest = 16,
        Exception = 32,
        NoData = 64,
        RetryTimeout = 128,
        NoNetwork = 256
    }
}
