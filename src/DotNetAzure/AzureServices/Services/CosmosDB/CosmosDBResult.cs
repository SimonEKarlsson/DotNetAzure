using System.Net;

namespace AzureServices.Services.CosmosDB
{
    public abstract class CosmosDBResult<T>
    {
        public CosmosDBResultCode StatusCode { get; }
        public List<string> Messages { get; }
        public T? Value { get; }
        public bool HasValue => !EqualityComparer<T>.Default.Equals(Value, default);

        protected CosmosDBResult(CosmosDBResultCode statusCode, List<string> messages, T? value)
        {
            StatusCode = statusCode;
            Messages = messages ?? new List<string>(); // Ensure ErrorMessages is never null
            Value = value;
        }
    }

    public class CosmosDBSuccessResult<T> : CosmosDBResult<T>
    {
        public CosmosDBSuccessResult(List<string> successMessages, T value) : base(CosmosDBResultCode.OK, successMessages, value)
        {

        }
    }

    public class CosmosDBEmptySuccessResult<T> : CosmosDBResult<T>
    {
        public CosmosDBEmptySuccessResult(List<string> successMessages) : base(CosmosDBResultCode.NoContent, successMessages, default)
        {

        }
    }

    public class CosmosDBErrorResult<T> : CosmosDBResult<T>
    {
        public CosmosDBErrorResult(List<string> errorMessages, CosmosDBResultCode statusCode) : base(statusCode, errorMessages, default)
        {

        }
    }

    public enum CosmosDBResultCode { OK, NoContent, Error, BadRequest }
}
