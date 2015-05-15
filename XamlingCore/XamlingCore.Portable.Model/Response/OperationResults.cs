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
        Success = 0,
        Failed = 1,
        NotFound = 2,
        NotAuthorised = 4,
        BadRequest = 8,
        Exception = 16,
        NoData = 32,
        RetryTimeout = 64
    }
}
