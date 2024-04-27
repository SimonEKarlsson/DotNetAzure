using System.Net;

namespace AzureServices.Services.AAD
{
    public abstract class AADResult<T>
    {
        public AADResultCode StatusCode { get; }
        public List<string> Messages { get; }
        public T Value { get; }
        public bool HasValue => !EqualityComparer<T>.Default.Equals(Value, default(T));

        protected AADResult(AADResultCode statusCode, List<string> messages, T value)
        {
            StatusCode = statusCode;
            Messages = messages ?? new List<string>(); // Ensure ErrorMessages is never null
            Value = value;
        }
    }

    public class AADSuccessResult<T> : AADResult<T>
    {
        public AADSuccessResult(List<string> successMessages, T value) : base(AADResultCode.OK, successMessages, value)
        {
        }
    }

    public class AADEmptySuccessResult<T> : AADResult<T>
    {
        public AADEmptySuccessResult(List<string> successMessages) : base(AADResultCode.NoContent, successMessages, default(T) ?? throw new Exception($"value {typeof(T)} is null"))
        {
        }
    }

    public class AADErrorResult<T> : AADResult<T>
    {
        public AADErrorResult(List<string> errorMessages, AADResultCode statusCode) : base(statusCode, errorMessages, default(T) ?? throw new Exception($"value {typeof(T)} is null"))
        {
        }
    }
    public enum AADResultCode { OK, NoContent, Error }
}
