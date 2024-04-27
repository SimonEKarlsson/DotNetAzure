using System.Net;

namespace AzureServices.Services.ServiceBus
{
    public abstract class ServiceBusResult<T>
    {
        public ServiceBusResultCode StatusCode { get; }
        public List<string> Messages { get; }
        public T Value { get; }
        public bool HasValue => !EqualityComparer<T>.Default.Equals(Value, default(T));

        protected ServiceBusResult(ServiceBusResultCode statusCode, List<string> messages, T value)
        {
            StatusCode = statusCode;
            Messages = messages ?? new List<string>(); // Ensure ErrorMessages is never null
            Value = value;
        }
    }

    public class ServiceBusSuccessResult<T> : ServiceBusResult<T>
    {
        public ServiceBusSuccessResult(List<string> successMessages, T value) : base(ServiceBusResultCode.OK, successMessages, value)
        {
        }
    }

    public class ServiceBusEmptySuccessResult<T> : ServiceBusResult<T>
    {
        public ServiceBusEmptySuccessResult(List<string> successMessages) : base(ServiceBusResultCode.NoContent, successMessages, default(T) ?? throw new Exception($"value {typeof(T)} is null"))
        {
        }
    }

    public class ServiceBusErrorResult<T> : ServiceBusResult<T>
    {
        public ServiceBusErrorResult(List<string> errorMessages, ServiceBusResultCode statusCode) : base(statusCode, errorMessages, default(T) ?? throw new Exception($"value {typeof(T)} is null"))
        {
        }
    }
    public enum ServiceBusResultCode { OK, NoContent, Error }
}
