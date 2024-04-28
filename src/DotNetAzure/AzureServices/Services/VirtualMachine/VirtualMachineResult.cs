using System.Net;

namespace AzureServices.Services.VirtualMachine
{
    public abstract class VirtualMachineResult<T>
    {
        public VirtualMachineResultCode StatusCode { get; }
        public List<string> Messages { get; }
        public T? Value { get; }
        public bool HasValue => !EqualityComparer<T>.Default.Equals(Value, default);

        protected VirtualMachineResult(VirtualMachineResultCode statusCode, List<string> messages, T? value)
        {
            StatusCode = statusCode;
            Messages = messages ?? new List<string>(); // Ensure ErrorMessages is never null
            Value = value;
        }
    }

    public class VirtualMachineSuccessResult<T> : VirtualMachineResult<T>
    {
        public VirtualMachineSuccessResult(List<string> successMessages, T value) : base(VirtualMachineResultCode.OK, successMessages, value)
        {
        }
    }

    public class VirtualMachineEmptySuccessResult<T> : VirtualMachineResult<T>
    {
        public VirtualMachineEmptySuccessResult(List<string> successMessages) : base(VirtualMachineResultCode.NoContent, successMessages, default)
        {
        }
    }

    public class VirtualMachineErrorResult<T> : VirtualMachineResult<T>
    {
        public VirtualMachineErrorResult(List<string> errorMessages, VirtualMachineResultCode statusCode) : base(statusCode, errorMessages, default)
        {
        }
    }
    public enum VirtualMachineResultCode { OK, NoContent, Error }
}
