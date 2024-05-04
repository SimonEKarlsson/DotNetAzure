using System.Net;

namespace AzureServices.Services.IoTHub
{
    public abstract class IoTHubResult<T>
    {
        public IoTHubResultCode StatusCode { get; }
        public List<string> Messages { get; }
        public T? Value { get; }
        public bool HasValue => !EqualityComparer<T>.Default.Equals(Value, default);
        public string StringMessages => string.Join("\n", Messages);

        protected IoTHubResult(IoTHubResultCode statusCode, List<string> messages, T? value)
        {
            StatusCode = statusCode;
            Messages = messages ?? new List<string>(); // Ensure ErrorMessages is never null
            Value = value;
        }
    }

    public class IoTHubSuccessResult<T> : IoTHubResult<T>
    {
        public IoTHubSuccessResult(List<string> successMessages, T value) : base(IoTHubResultCode.OK, successMessages, value)
        {
        }
    }

    public class IoTHubEmptySuccessResult<T> : IoTHubResult<T>
    {
        public IoTHubEmptySuccessResult(List<string> successMessages) : base(IoTHubResultCode.NoContent, successMessages, default)
        {
        }
    }

    public class IoTHubErrorResult<T> : IoTHubResult<T>
    {
        public IoTHubErrorResult(List<string> errorMessages, IoTHubResultCode statusCode) : base(statusCode, errorMessages, default)
        {
        }
    }
    public enum IoTHubResultCode { OK, NoContent, Error }
}
