using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XamlingCore.Portable.Model.Response;

namespace XamlingCore.Portable.Model.Resiliency
{
    public class XResiliant
    {
        static XResiliant()
        {
            Default = new XResiliant();
            Exception = new XResiliant(exceptionsOnly: true);
        }

        public static XResiliant Default { get; private set; }
        public static XResiliant Exception { get; private set; }

        public XResiliant(int retries = 1, int retryTimeout = 1000,
                OperationResults allowedResultsCodes = OperationResults.Success | OperationResults.NotAuthorised | OperationResults.NotFound,
                bool exceptionsOnly = false)
        {
            Retries = retries;
            RetryTimeout = retryTimeout;
            AllowedResultCodes = allowedResultsCodes;
            ExceptionsOnly = exceptionsOnly;
        }

        public int Retries { get; set; }
        public int RetryTimeout { get; set; }

        public bool ExceptionsOnly { get; set; }

        public OperationResults AllowedResultCodes { get; set; }


        public async Task<XResult<T>> Run<T>(Func<Task<XResult<T>>> func)
        {
            var counter = 0;

            XResult<T> lastResult = null;

            while (counter <= Retries || Retries == -1)
            {
                try
                {
                    var result = await func();

                    result.Retries = counter;

                    if (ExceptionsOnly)
                    {
                        return result;
                    }

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

                    counter++;
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

        public async Task<XResult<T>> Run<T>(Func<Task<T>> func)
            where T:class 
        {
            var run = await Run(async () =>
            {
                var result = await func();

                if (result == null)
                {
                    return XResult<T>.GetFailed();
                }

                return new XResult<T>(result);
            });
            return run;
        }

        public async Task<XResult<bool>> RunBool(Func<Task<bool>> func)
        {
            return await Run(async () =>
            {
                var result = await func();

                if (!result)
                {
                    return XResult<bool>.GetFailed();
                }

                return new XResult<bool>(true);
            });
        }
    }
}
