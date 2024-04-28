using System.Net;

namespace AzureServices.Services.SQLDB
{
    public abstract class SQLDBResult<T>
    {
        public SQLDBResultCode StatusCode { get; }
        public List<string> Messages { get; }
        public T? Value { get; }
        public bool HasValue => !EqualityComparer<T>.Default.Equals(Value, default);

        protected SQLDBResult(SQLDBResultCode statusCode, List<string> messages, T? value)
        {
            StatusCode = statusCode;
            Messages = messages ?? new List<string>(); // Ensure ErrorMessages is never null
            Value = value;
        }
    }

    public class SQLDBSuccessResult<T> : SQLDBResult<T>
    {
        public SQLDBSuccessResult(List<string> successMessages, T value) : base(SQLDBResultCode.OK, successMessages, value)
        {
        }
    }

    public class SQLDBEmptySuccessResult<T> : SQLDBResult<T>
    {
        public SQLDBEmptySuccessResult(List<string> successMessages) : base(SQLDBResultCode.NoContent, successMessages, default)
        {
        }
    }

    public class SQLDBErrorResult<T> : SQLDBResult<T>
    {
        public SQLDBErrorResult(List<string> errorMessages, SQLDBResultCode statusCode) : base(statusCode, errorMessages, default)
        {
        }
    }
    public enum SQLDBResultCode { OK, NoContent, Error }
}
