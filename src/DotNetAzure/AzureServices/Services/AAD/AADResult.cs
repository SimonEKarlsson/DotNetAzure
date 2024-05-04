namespace AzureServices.Services.AAD
{
    public abstract class AADResult<T>
    {
        public AADResultCode StatusCode { get; }
        public List<string> Messages { get; }
        public T? Value { get; }
        public bool HasValue => !EqualityComparer<T>.Default.Equals(Value, default);
        public string StringMessages => string.Join("\n", Messages);

        protected AADResult(AADResultCode statusCode, List<string> messages, T? value)
        {
            StatusCode = statusCode;
            Messages = messages ?? new List<string>(); // Ensure ErrorMessages is never null
            Value = value;
        }
    }

    public class AADSuccessResult<T> : AADResult<T>
    {
        public AADSuccessResult(List<string> successMessages, T value) : base(AADResultCode.OK, successMessages, value)
        {
        }
    }

    public class AADEmptySuccessResult<T> : AADResult<T>
    {
        public AADEmptySuccessResult(List<string> successMessages) : base(AADResultCode.NoContent, successMessages, default)
        {
        }
    }

    public class AADErrorResult<T> : AADResult<T>
    {
        public AADErrorResult(List<string> errorMessages, AADResultCode statusCode) : base(statusCode, errorMessages, default)
        {
        }
    }
    public enum AADResultCode { OK, NoContent, Error, BadRequest }
}
