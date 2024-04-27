using System.Net;

namespace AzureServices.Services.Keyvault
{
    public abstract class KeyvaultResult<T>
    {
        public HttpStatusCode StatusCode { get; }
        public List<string> Messages { get; }
        public T Value { get; }
        public bool HasValue => !EqualityComparer<T>.Default.Equals(Value, default(T));

        protected KeyvaultResult(HttpStatusCode statusCode, List<string> messages, T value)
        {
            StatusCode = statusCode;
            Messages = messages ?? new List<string>(); // Ensure ErrorMessages is never null
            Value = value;
        }
    }

    public class KeyvaultSuccessResult<T> : KeyvaultResult<T>
    {
        public KeyvaultSuccessResult(List<string> successMessages, T value) : base(HttpStatusCode.OK, successMessages, value)
        {
        }
    }

    public class KeyvaultEmptySuccessResult<T> : KeyvaultResult<T>
    {
        public KeyvaultEmptySuccessResult(List<string> successMessages) : base(HttpStatusCode.NoContent, successMessages, default(T) ?? throw new Exception($"value {typeof(T)} is null"))
        {
        }
    }

    public class KeyvaultErrorResult<T> : KeyvaultResult<T>
    {
        public KeyvaultErrorResult(List<string> errorMessages, HttpStatusCode statusCode) : base(statusCode, errorMessages, default(T) ?? throw new Exception($"value {typeof(T)} is null"))
        {
        }
    }
}
