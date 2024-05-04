using AzureServices.Services.OAuth2Token;

namespace AzureServices.Services.OAuth2Token
{
    public class OAuth2TokenResult<T>
    {
        public OAuth2TokenResultCode StatusCode { get; }
        public List<string> Messages { get; }
        public T? Value { get; }
        public bool HasValue => !EqualityComparer<T>.Default.Equals(Value, default);
        public string StringMessages => string.Join("\n", Messages);

        protected OAuth2TokenResult(OAuth2TokenResultCode statusCode, List<string> messages, T? value)
        {
            StatusCode = statusCode;
            Messages = messages ?? new List<string>(); // Ensure ErrorMessages is never null
            Value = value;
        }
    }

    public class OAuth2TokenSuccessResult<T> : OAuth2TokenResult<T>
    {
        public OAuth2TokenSuccessResult(List<string> successMessages, T value) : base(OAuth2TokenResultCode.OK, successMessages, value)
        {
        }
    }

    public class OAuth2TokenEmptySuccessResult<T> : OAuth2TokenResult<T>
    {
        public OAuth2TokenEmptySuccessResult(List<string> successMessages) : base(OAuth2TokenResultCode.NoContent, successMessages, default)
        {
        }
    }

    public class OAuth2TokenErrorResult<T> : OAuth2TokenResult<T>
    {
        public OAuth2TokenErrorResult(List<string> errorMessages, OAuth2TokenResultCode statusCode) : base(statusCode, errorMessages, default)
        {
        }
    }
    public enum OAuth2TokenResultCode { OK, NoContent, Error, BadRequest }
}
