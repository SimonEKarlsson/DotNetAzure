using System.Net;

namespace AzureServices.Services.EventHub
{
    public abstract class EventHubResult<T>
    {
        public HttpStatusCode StatusCode { get; }
        public List<string> Messages { get; }
        public T Value { get; }
        public bool HasValue => !EqualityComparer<T>.Default.Equals(Value, default(T));

        protected EventHubResult(HttpStatusCode statusCode, List<string> messages, T value)
        {
            StatusCode = statusCode;
            Messages = messages ?? new List<string>(); // Ensure ErrorMessages is never null
            Value = value;
        }
    }

    public class EventHubSuccessResult<T> : EventHubResult<T>
    {
        public EventHubSuccessResult(List<string> successMessages, T value) : base(HttpStatusCode.OK, successMessages, value)
        {
        }
    }

    public class EventHubEmptySuccessResult<T> : EventHubResult<T>
    {
        public EventHubEmptySuccessResult(List<string> successMessages) : base(HttpStatusCode.NoContent, successMessages, default(T) ?? throw new Exception($"value {typeof(T)} is null"))
        {
        }
    }

    public class EventHubErrorResult<T> : EventHubResult<T>
    {
        public EventHubErrorResult(List<string> errorMessages, HttpStatusCode statusCode) : base(statusCode, errorMessages, default(T) ?? throw new Exception($"value {typeof(T)} is null"))
        {
        }
    }
}
