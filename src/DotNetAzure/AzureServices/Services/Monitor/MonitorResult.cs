using System.Net;

namespace AzureServices.Services.Monitor
{
    public abstract class MonitorResult<T>
    {
        public MonitorResultCode StatusCode { get; }
        public List<string> Messages { get; }
        public T? Value { get; }
        public bool HasValue => !EqualityComparer<T>.Default.Equals(Value, default);
        public string StringMessages => string.Join("\n", Messages);

        protected MonitorResult(MonitorResultCode statusCode, List<string> messages, T? value)
        {
            StatusCode = statusCode;
            Messages = messages ?? new List<string>(); // Ensure ErrorMessages is never null
            Value = value;
        }
    }

    public class MonitorSuccessResult<T> : MonitorResult<T>
    {
        public MonitorSuccessResult(List<string> successMessages, T value) : base(MonitorResultCode.OK, successMessages, value)
        {
        }
    }

    public class MonitorEmptySuccessResult<T> : MonitorResult<T>
    {
        public MonitorEmptySuccessResult(List<string> successMessages) : base(MonitorResultCode.NoContent, successMessages, default)
        {
        }
    }

    public class MonitorErrorResult<T> : MonitorResult<T>
    {
        public MonitorErrorResult(List<string> errorMessages, MonitorResultCode statusCode) : base(statusCode, errorMessages, default)
        {
        }
    }
    public enum MonitorResultCode { OK, NoContent, Error }
}
