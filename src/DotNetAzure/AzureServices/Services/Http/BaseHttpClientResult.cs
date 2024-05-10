using System.Net;

namespace AzureServices.Services.Http
{
    public abstract class BaseHttpResult<T>
    {
        public HttpStatusCode StatusCode { get; }
        public List<string> Messages { get; }
        public T? Value { get; }
        public bool HasValue => !EqualityComparer<T>.Default.Equals(Value, default);
        public string StringMessages => string.Join("\n", Messages);

        protected BaseHttpResult(HttpStatusCode statusCode, List<string> messages, T? value)
        {
            StatusCode = statusCode;
            Messages = messages ?? new List<string>(); // Ensure ErrorMessages is never null
            Value = value;
        }
    }

    public class BaseHttpSuccessResult<T> : BaseHttpResult<T>
    {
        public BaseHttpSuccessResult(List<string> successMessages, T value) : base(HttpStatusCode.OK, successMessages, value)
        {
        }
    }

    public class BaseHttpEmptySuccessResult<T> : BaseHttpResult<T>
    {
        public BaseHttpEmptySuccessResult(List<string> successMessages) : base(HttpStatusCode.NoContent, successMessages, default)
        {
        }
    }

    public class BaseHttpErrorResult<T> : BaseHttpResult<T>
    {
        public BaseHttpErrorResult(List<string> errorMessages, HttpStatusCode statusCode) : base(statusCode, errorMessages, default)
        {
        }
    }
}
