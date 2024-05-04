using System.Net;

namespace AzureServices.Services.StorageAccount
{
    public abstract class StorageAccountResult<T>
    {
        public StorageAccountResultCode StatusCode { get; }
        public List<string> Messages { get; }
        public T? Value { get; }
        public bool HasValue => !EqualityComparer<T>.Default.Equals(Value, default);
        public string StringMessages => string.Join("\n", Messages);

        protected StorageAccountResult(StorageAccountResultCode statusCode, List<string> messages, T? value)
        {
            StatusCode = statusCode;
            Messages = messages ?? new List<string>(); // Ensure ErrorMessages is never null
            Value = value;
        }
    }

    public class StorageAccountSuccessResult<T> : StorageAccountResult<T>
    {
        public StorageAccountSuccessResult(List<string> successMessages, T value) : base(StorageAccountResultCode.OK, successMessages, value)
        {
        }
    }

    public class StorageAccountEmptySuccessResult<T> : StorageAccountResult<T>
    {
        public StorageAccountEmptySuccessResult(List<string> successMessages) : base(StorageAccountResultCode.NoContent, successMessages, default)
        {
        }
    }

    public class StorageAccountErrorResult<T> : StorageAccountResult<T>
    {
        public StorageAccountErrorResult(List<string> errorMessages, StorageAccountResultCode statusCode) : base(statusCode, errorMessages, default)
        {
        }
    }
    public enum StorageAccountResultCode { OK, NoContent, Error, BadRequset }
}
