using System.Net;

namespace AzureServices.Services.EventHub
{
    public abstract class EventHubResult<T>
    {
        public EventHubResultCode StatusCode { get; }
        public List<string> Messages { get; }
        public T? Value { get; }
        public bool HasValue => !EqualityComparer<T>.Default.Equals(Value, default);
        public string StringMessages => string.Join("\n", Messages);

        protected EventHubResult(EventHubResultCode statusCode, List<string> messages, T? value)
        {
            StatusCode = statusCode;
            Messages = messages ?? new List<string>(); // Ensure ErrorMessages is never null
            Value = value;
        }
    }

    public class EventHubSuccessResult<T> : EventHubResult<T>
    {
        public EventHubSuccessResult(List<string> successMessages, T value) : base(EventHubResultCode.OK, successMessages, value)
        {
        }
    }

    public class EventHubEmptySuccessResult<T> : EventHubResult<T>
    {
        public EventHubEmptySuccessResult(List<string> successMessages) : base(EventHubResultCode.NoContent, successMessages, default)
        {
        }
    }

    public class EventHubErrorResult<T> : EventHubResult<T>
    {
        public EventHubErrorResult(List<string> errorMessages, EventHubResultCode statusCode) : base(statusCode, errorMessages, default)
        {
        }
    }
    public enum EventHubResultCode { OK, NoContent, Error }
}
