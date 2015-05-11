using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace XamlingCore.Portable.Model.Response
{

    public class OperationResult<T>
    {
        public OperationResult()
        {

        }

        public OperationResult(T obj, string message = null, OperationResults result = OperationResults.Success,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
            [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0
            )
        {
            Result = result;
            Object = obj;
            Message = message;
            _setCallerInformation(memberName, sourceFilePath, sourceLineNumber);
        }

        public OperationResult(T obj, bool isSuccess, string message = null,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
            [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0
            )
        {
            Result = isSuccess ? OperationResults.Success : OperationResults.Failed;
            Object = obj;
            Message = message;
            _setCallerInformation(memberName, sourceFilePath, sourceLineNumber);
        }

        public OperationResult(string message, OperationResults result,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
            [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0
            )
        {
            Result = result;
            Message = message;
            _setCallerInformation(memberName, sourceFilePath, sourceLineNumber);
        }

        public OperationResult<TOther> Copy<TOther>(TOther obj = default(TOther),
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
            [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0
            )
        {
            var o = new OperationResult<TOther>(obj, Message, Result);

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

        [JsonProperty(PropertyName = "data", NullValueHandling = NullValueHandling.Ignore)]
        public T Object { get; set; }

        [JsonProperty(PropertyName = "message", NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; set; }

        [JsonProperty(PropertyName = "success", NullValueHandling = NullValueHandling.Ignore)]
        public bool IsSuccess
        {
            get
            {
                return Result == OperationResults.Success;
            }
        }

        [JsonProperty(PropertyName = "ex", NullValueHandling = NullValueHandling.Ignore)]
        public Exception Exception { get; set; }
        [JsonProperty(PropertyName = "status", NullValueHandling = NullValueHandling.Ignore)]
        public int StatusCode { get; set; }

        [JsonProperty(PropertyName = "code", NullValueHandling = NullValueHandling.Ignore)]
        public OperationResults Result { get; set; }

        [JsonProperty(PropertyName = "d", NullValueHandling = NullValueHandling.Ignore)]
        public OperationCallerInfo CallerInfo { get; set; }

        [JsonProperty(PropertyName = "d_history", NullValueHandling = NullValueHandling.Ignore)]
        public List<OperationCallerInfo> CallerInfoHistory { get; set; }

        public static OperationResult<T> GetSuccess(T obj,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
            [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
        {
            var o = new OperationResult<T>(obj);
            o._setCallerInformation(memberName, sourceFilePath, sourceLineNumber);
            return o;
        }

        public static OperationResult<T> GetNotFound(string message = null,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
            [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0
            )
        {
            var o = new OperationResult<T>(default(T), message ?? "Object not found",
                OperationResults.NotFound);

            o._setCallerInformation(memberName, sourceFilePath, sourceLineNumber);

            return o;
        }

        public static OperationResult<T> GetNotAuthorised(string message = null,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
            [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0
            )
        {
            var o = new OperationResult<T>(default(T), message ?? "Not authorised to do that",
                OperationResults.NotAuthorised);

            o._setCallerInformation(memberName, sourceFilePath, sourceLineNumber);

            return o;
        }

        public static OperationResult<T> GetBadRequest(string message = null,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
            [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0
            )
        {
            var o = new OperationResult<T>(default(T), message ?? "It doesn't work like that (bad request)",
                OperationResults.BadRequest);

            o._setCallerInformation(memberName, sourceFilePath, sourceLineNumber);

            return o;
        }

        public static OperationResult<T> GetException(string message = null, Exception ex = null,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
            [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0
            )
        {
            var messageFormatted = string.Format("Message: {0}, Exception: {1}", message, ex);

            var o = new OperationResult<T>(default(T), messageFormatted,
                OperationResults.Exception);
            
            o.Exception = ex;
            
            o._setCallerInformation(memberName, sourceFilePath, sourceLineNumber);

            return o;
        }

        public static OperationResult<T> GetFailed(string message = null,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
            [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0
            )
        {
            var o = new OperationResult<T>(default(T), message ?? "Something went wrong and I could not complete that",
                OperationResults.Failed);

            o._setCallerInformation(memberName, sourceFilePath, sourceLineNumber);

            return o;
        }

        public static OperationResult<T> GetDatabaseProblem(string message = null,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
            [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
        {
            var o = new OperationResult<T>(default(T), string.Format("Database problem {0}", message),
                OperationResults.Failed);

            o._setCallerInformation(memberName, sourceFilePath, sourceLineNumber);

            return o;
        }

        public static OperationResult<T> GetNoData(string message = null,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
            [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
        {
            var o = new OperationResult<T>(default(T), string.Format("No data returned - possible network issue {0}", message),
                OperationResults.NoData);

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

    public enum OperationResults
    {
        Success,
        Failed,
        NotFound,
        NotAuthorised,
        BadRequest,
        Exception,
        NoData
    }
}
