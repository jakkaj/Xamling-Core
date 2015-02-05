using Newtonsoft.Json;

namespace XamlingCore.Portable.Model.Response
{
    public class OperationResult<T>
    {
        public OperationResult()
        {
            
        }

        public OperationResult(T obj, string message = null, OperationResults result = OperationResults.Success)
        {
            Result = result;
            Object = obj;
            Message = message;
        }

        public OperationResult(T obj, bool isSuccess, string message = null)
        {
            Result = isSuccess ? OperationResults.Success : OperationResults.OperationFailed;
            Object = obj;
            Message = message;
        }

        public OperationResult(string message, OperationResults result)
        {
            Result = result;
            Message = message;
        }

        public OperationResult<TOther> Copy<TOther>(TOther obj)
        {
            return new OperationResult<TOther>(obj, Message, Result);
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

        public static OperationResult<T> GetSuccess(T obj)
        {
            return new OperationResult<T>(obj);
        }

        public static OperationResult<T> GetNotFound(string message = null)
        {
            var o = new OperationResult<T>(default(T), message ?? "Object not found",
                OperationResults.NotFound);

            return o;
        }

        public static OperationResult<T> GetNotAuthorised(string message = null)
        {
            var o = new OperationResult<T>(default(T), message ?? "Not authorised to do that",
                OperationResults.NotAuthorised);

            return o;
        }

        public static OperationResult<T> GetBadRequest(string message = null)
        {
            var o = new OperationResult<T>(default(T), message ?? "It doesn't work like that (bad request)",
                OperationResults.BadRequest);

            return o;
        }

        public static OperationResult<T> GetOperationFailed(string message = null)
        {
            var o = new OperationResult<T>(default(T), message ?? "Something went wrong and I could not complete that",
                OperationResults.OperationFailed);

            return o;
        }

        public static OperationResult<T> GetDatabaseProblem(string message = null)
        {
            var o = new OperationResult<T>(default(T), string.Format("Database problem {0}", message),
                OperationResults.OperationFailed);

            return o;
        }

        public static OperationResult<T> GetNoData(string message = null)
        {
            var o = new OperationResult<T>(default(T), string.Format("No data returned - possible network issue {0}", message),
                OperationResults.NoData);

            return o;
        }
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
