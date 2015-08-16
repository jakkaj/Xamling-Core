using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace XamlingCore.Portable.Model.Response
{

    public class XResult<T>
    {
        public XResult()
        {
            Id = Guid.NewGuid();
        }

        public static implicit operator bool(XResult<T> operation)
        {
            return operation.IsSuccess;
        }

        public XResult(T obj, string message = null, OperationResults result = OperationResults.Success,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
            [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0
            )
        {
            ResultCode = result;
            Object = obj;
            Message = message;
            _setCallerInformation(memberName, sourceFilePath, sourceLineNumber);
        }

        public XResult(T obj, bool isSuccess, string message = null,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
            [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0
            )
        {
            ResultCode = isSuccess ? OperationResults.Success : OperationResults.Failed;
            Object = obj;
            Message = message;
            _setCallerInformation(memberName, sourceFilePath, sourceLineNumber);
        }

        public XResult(string message, OperationResults result,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
            [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0
            )
        {
            ResultCode = result;
            Message = message;
            _setCallerInformation(memberName, sourceFilePath, sourceLineNumber);
        }

        public XResult<TOther> Copy<TOther>(TOther obj = default(TOther),
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
            [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0
            )
        {
            var o = new XResult<TOther>(obj, Message, ResultCode);

            if (o.CallerInfoHistory == null)
            {
                o.CallerInfoHistory = new List<OperationCallerInfo>();
            }

            o.CallerInfoHistory.Add(CallerInfo);

            if (CallerInfoHistory != null)
            {
                foreach (var item in CallerInfoHistory)
                {
                    o.CallerInfoHistory.Add(item);
                }
            }

            o._setCallerInformation(memberName, sourceFilePath, sourceLineNumber);
            return o;
        }

        [JsonProperty(PropertyName = "id", NullValueHandling = NullValueHandling.Ignore)]
        public Guid? Id { get; set; }


        [JsonProperty(PropertyName = "data", NullValueHandling = NullValueHandling.Ignore)]
        public T Object { get; set; }

        [JsonProperty(PropertyName = "message", NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; set; }

        [JsonProperty(PropertyName = "success", NullValueHandling = NullValueHandling.Ignore)]
        public bool IsSuccess
        {
            get
            {
                return ResultCode == OperationResults.Success;
            }
        }

        [JsonProperty(PropertyName = "ret", NullValueHandling = NullValueHandling.Ignore)]
        public int? Retries { get; set; }

        [JsonProperty(PropertyName = "ex", NullValueHandling = NullValueHandling.Ignore)]
        public Exception Exception { get; set; }
        [JsonProperty(PropertyName = "status", NullValueHandling = NullValueHandling.Ignore)]
        public int StatusCode { get; set; }

        [JsonProperty(PropertyName = "code", NullValueHandling = NullValueHandling.Ignore)]
        public OperationResults ResultCode { get; set; }

        [JsonProperty(PropertyName = "d", NullValueHandling = NullValueHandling.Ignore)]
        public OperationCallerInfo CallerInfo { get; set; }

        [JsonProperty(PropertyName = "d_history", NullValueHandling = NullValueHandling.Ignore)]
        public List<OperationCallerInfo> CallerInfoHistory { get; set; }

        public static XResult<T> GetSuccess(T obj,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
            [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
        {
            var o = new XResult<T>(obj);
            o._setCallerInformation(memberName, sourceFilePath, sourceLineNumber);
            return o;
        }

        public static XResult<T> GetNotFound(string message = null,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
            [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0
            )
        {
            var o = new XResult<T>(default(T), message ?? "Object not found",
                OperationResults.NotFound);

            o._setCallerInformation(memberName, sourceFilePath, sourceLineNumber);

            return o;
        }

        public static XResult<T> GetNotAuthorised(string message = null,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
            [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0
            )
        {
            var o = new XResult<T>(default(T), message ?? "Not authorised to do that",
                OperationResults.NotAuthorised);

            o._setCallerInformation(memberName, sourceFilePath, sourceLineNumber);

            return o;
        }

        public static XResult<T> GetBadRequest(string message = null,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
            [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0
            )
        {
            var o = new XResult<T>(default(T), message ?? "It doesn't work like that (bad request)",
                OperationResults.BadRequest);

            o._setCallerInformation(memberName, sourceFilePath, sourceLineNumber);

            return o;
        }

        public static XResult<T> GetException(string message = null, Exception ex = null,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
            [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0
            )
        {
            var messageFormatted = string.Format("Message: {0}, Exception: {1}", message, ex);

            var o = new XResult<T>(default(T), messageFormatted,
                OperationResults.Exception);
            
            o.Exception = ex;
            
            o._setCallerInformation(memberName, sourceFilePath, sourceLineNumber);

            return o;
        }

        public static XResult<T> GetFailed(string message = null,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
            [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0
            )
        {
            var o = new XResult<T>(default(T), message ?? "Something went wrong and I could not complete that",
                OperationResults.Failed);

            o._setCallerInformation(memberName, sourceFilePath, sourceLineNumber);

            return o;
        }

        public static XResult<T> GetDatabaseProblem(string message = null,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
            [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
        {
            var o = new XResult<T>(default(T), string.Format("Database problem {0}", message),
                OperationResults.Failed);

            o._setCallerInformation(memberName, sourceFilePath, sourceLineNumber);

            return o;
        }

        public static XResult<T> GetNoData(string message = null,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
            [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
        {
            var o = new XResult<T>(default(T), string.Format("No data returned - possible network issue {0}", message),
                OperationResults.NoData);

            o._setCallerInformation(memberName, sourceFilePath, sourceLineNumber);

            return o;
        }

        public static XResult<T> GetNoRecord(string message = null,
           [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
           [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
           [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
        {
            var o = new XResult<T>(default(T), string.Format("No record found that matches your request", message),
                OperationResults.NoRecord);

            o._setCallerInformation(memberName, sourceFilePath, sourceLineNumber);

            return o;
        }

        void _setCallerInformation(string memberName, string sourceFilePath, int sourceLineNumber)
        {
            CallerInfo = new OperationCallerInfo(memberName, sourceFilePath, sourceLineNumber);
        }

       

     

    }

    public class OperationCallerInfo
    {
        public OperationCallerInfo(string memberName, string sourceFilePath, int sourceLineNumber)
        {
            MemberName = memberName;
            SourceFilePath = sourceFilePath;
            SourceLineNumber = sourceLineNumber;
        }

        public string MemberName { get; private set; }

        public string SourceFilePath { get; private set; }

        public int SourceLineNumber { get; private set; }
    }

    
}
