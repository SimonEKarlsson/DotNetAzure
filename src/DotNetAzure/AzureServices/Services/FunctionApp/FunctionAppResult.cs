using System.Net;

namespace AzureServices.Services.FunctionApp
{
    public abstract class FunctionAppResult<T>
    {
        public FunctionAppResultCode StatusCode { get; }
        public List<string> Messages { get; }
        public T Value { get; }
        public bool HasValue => !EqualityComparer<T>.Default.Equals(Value, default(T));

        protected FunctionAppResult(FunctionAppResultCode statusCode, List<string> messages, T value)
        {
            StatusCode = statusCode;
            Messages = messages ?? new List<string>(); // Ensure ErrorMessages is never null
            Value = value;
        }
    }

    public class FunctionAppSuccessResult<T> : FunctionAppResult<T>
    {
        public FunctionAppSuccessResult(List<string> successMessages, T value) : base(FunctionAppResultCode.OK, successMessages, value)
        {
        }
    }

    public class FunctionAppEmptySuccessResult<T> : FunctionAppResult<T>
    {
        public FunctionAppEmptySuccessResult(List<string> successMessages) : base(FunctionAppResultCode.NoContent, successMessages, default(T) ?? throw new Exception($"value {typeof(T)} is null"))
        {
        }
    }

    public class FunctionAppErrorResult<T> : FunctionAppResult<T>
    {
        public FunctionAppErrorResult(List<string> errorMessages, FunctionAppResultCode statusCode) : base(statusCode, errorMessages, default(T) ?? throw new Exception($"value {typeof(T)} is null"))
        {
        }
    }
    public enum FunctionAppResultCode { OK, NoContent, Error }
}
