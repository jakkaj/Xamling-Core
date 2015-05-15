using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XamlingCore.Portable.Model.Response;

namespace XamlingCore.Portable.Model.Resiliency
{
    public class ResiliantOperation<T>
    {
        public ResiliantOperation(int retries = 1, int retryTimeout = 1000,
            OperationResults allowedResultsCodes = OperationResults.Success | OperationResults.NotAuthorised | OperationResults.NotFound)
        {
            Retries = retries;
            RetryTimeout = retryTimeout;
            AllowedResultCodes = allowedResultsCodes;
        }

        public int Retries { get; set; }
        public int RetryTimeout { get; set; }

        public OperationResults AllowedResultCodes { get; set; }


        public async Task<XResult<T>> Run(Func<Task<XResult<T>>> func)
        {
            var counter = 0;

            XResult<T> lastResult = null;

            while (counter <= Retries || Retries == -1)
            {
                

                try
                {
                    var result = await func();

                    if (!result)
                    {
                        if (AllowedResultCodes.HasFlag(result.ResultCode))
                        {
                            return result;
                        }

                        lastResult = result;

                        if (counter == Retries)
                        {
                            break;
                        }
                        await Task.Delay(RetryTimeout);
                        counter++;
                        continue;
                    }

                    result.Retries = counter;
                    return result;
                }
                catch (Exception ex)
                {
                    lastResult = XResult<T>.GetException();
                    lastResult.Exception = ex;

                    if (counter == Retries)
                    {
                        break;
                    }

                    counter ++;
                }
            }

            if (lastResult == null)
            {
                lastResult = XResult<T>.GetFailed();
                lastResult.ResultCode = OperationResults.RetryTimeout; 
            }

            lastResult.Retries = counter;

            return lastResult;
        }
    }
}
