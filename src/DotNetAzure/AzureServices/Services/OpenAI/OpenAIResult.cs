using System.Net;

namespace AzureServices.Services.OpenAI
{
    public abstract class OpenAIResult<T>
    {
        public OpenAIResultCode StatusCode { get; }
        public List<string> Messages { get; }
        public T? Value { get; }
        public bool HasValue => !EqualityComparer<T>.Default.Equals(Value, default);

        protected OpenAIResult(OpenAIResultCode statusCode, List<string> messages, T? value)
        {
            StatusCode = statusCode;
            Messages = messages ?? new List<string>(); // Ensure ErrorMessages is never null
            Value = value;
        }
    }

    public class OpenAISuccessResult<T> : OpenAIResult<T>
    {
        public OpenAISuccessResult(List<string> successMessages, T value) : base(OpenAIResultCode.OK, successMessages, value)
        {
        }
    }

    public class OpenAIEmptySuccessResult<T> : OpenAIResult<T>
    {
        public OpenAIEmptySuccessResult(List<string> successMessages) : base(OpenAIResultCode.NoContent, successMessages, default)
        {
        }
    }

    public class OpenAIErrorResult<T> : OpenAIResult<T>
    {
        public OpenAIErrorResult(List<string> errorMessages, OpenAIResultCode statusCode) : base(statusCode, errorMessages, default)
        {
        }
    }
    public enum OpenAIResultCode { OK, NoContent, Error }
}
