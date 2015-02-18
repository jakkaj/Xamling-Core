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
            Result = isSuccess ? OperationResults.Success : OperationResults.OperationFailed;
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

        public OperationResult<TOther> Copy<TOther>(TOther obj,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
            [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0
            )
        {
            var o = new OperationResult<TOther>(obj, Message, Result);

            o.CallerInfoOriginal = CallerInfo;
            o._setCallerInformation(memberName, sourceFilePath, sourceLineNumber);
            return o;
        }

        [JsonProperty(PropertyName = "data")]
        public T Object { get; set; }

        [JsonProperty(PropertyName = "message")]
        public string Message { get; private set; }

        [JsonProperty(PropertyName = "success")]
        public bool IsSuccess
        {
            get
            {
                return Result == OperationResults.Success;
            }
        }

        [JsonProperty(PropertyName = "code")]
        public OperationResults Result { get; private set; }

        [JsonProperty(PropertyName = "d")]
        public OperationCallerInfo CallerInfo { get; set; }

        [JsonProperty(PropertyName = "d_orig")]
        public OperationCallerInfo CallerInfoOriginal { get; set; }

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

        public static OperationResult<T> GetOperationFailed(string message = null,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
            [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0
            )
        {
            var o = new OperationResult<T>(default(T), message ?? "Something went wrong and I could not complete that",
                OperationResults.OperationFailed);

            o._setCallerInformation(memberName, sourceFilePath, sourceLineNumber);

            return o;
        }

        public static OperationResult<T> GetDatabaseProblem(string message = null,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
            [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
        {
            var o = new OperationResult<T>(default(T), string.Format("Database problem {0}", message),
                OperationResults.OperationFailed);

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

        public string MemberName { get; }

        public string SourceFilePath { get; }

        public int SourceLineNumber { get; }
    }

    public enum OperationResults
    {
        Success,
        NotFound,
        NotAuthorised,
        BadRequest,
        OperationFailed,
        NoData
    }
}
