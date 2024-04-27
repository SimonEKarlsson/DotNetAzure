using System.Net;

namespace AzureServices.Services.VirtualMachine
{
    public abstract class VirtualMachineResult<T>
    {
        public HttpStatusCode StatusCode { get; }
        public List<string> Messages { get; }
        public T Value { get; }
        public bool HasValue => !EqualityComparer<T>.Default.Equals(Value, default(T));

        protected VirtualMachineResult(HttpStatusCode statusCode, List<string> messages, T value)
        {
            StatusCode = statusCode;
            Messages = messages ?? new List<string>(); // Ensure ErrorMessages is never null
            Value = value;
        }
    }

    public class VirtualMachineSuccessResult<T> : VirtualMachineResult<T>
    {
        public VirtualMachineSuccessResult(List<string> successMessages, T value) : base(HttpStatusCode.OK, successMessages, value)
        {
        }
    }

    public class VirtualMachineEmptySuccessResult<T> : VirtualMachineResult<T>
    {
        public VirtualMachineEmptySuccessResult(List<string> successMessages) : base(HttpStatusCode.NoContent, successMessages, default(T) ?? throw new Exception($"value {typeof(T)} is null"))
        {
        }
    }

    public class VirtualMachineErrorResult<T> : VirtualMachineResult<T>
    {
        public VirtualMachineErrorResult(List<string> errorMessages, HttpStatusCode statusCode) : base(statusCode, errorMessages, default(T) ?? throw new Exception($"value {typeof(T)} is null"))
        {
        }
    }
}
