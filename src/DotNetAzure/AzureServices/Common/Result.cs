using System.Net;
namespace AzureServices.Common
{
    public abstract class Result<T>
    {
        public HttpStatusCode StatusCode { get; }
        public List<string> Messages { get; }
        public T Value { get; }
        public bool HasValue => !EqualityComparer<T>.Default.Equals(Value, default(T));

        protected Result(HttpStatusCode statusCode, List<string> messages, T value)
        {
            StatusCode = statusCode;
            Messages = messages ?? new List<string>(); // Ensure ErrorMessages is never null
            Value = value;
        }
    }

    public class SuccessResult<T> : Result<T>
    {
        public SuccessResult(List<string> successMessages, T value) : base(HttpStatusCode.OK, successMessages, value)
        {
        }
    }

    public class EmptySuccessResult<T> : Result<T>
    {
        public EmptySuccessResult(List<string> successMessages) : base(HttpStatusCode.NoContent, successMessages, default(T) ?? throw new Exception($"value {typeof(T)} is null"))
        {
        }
    }

    public class ErrorResult<T> : Result<T>
    {
        public ErrorResult(List<string> errorMessages, HttpStatusCode statusCode) : base(statusCode, errorMessages, default(T) ?? throw new Exception($"value {typeof(T)} is null"))
        {
        }
    }
}
