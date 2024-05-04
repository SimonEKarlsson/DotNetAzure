namespace AzureServices.Services.AzureDefaultCredential
{
    public class AzureDefaultCredentialResult<T>
    {
        public AzureDefaultCredentialResultCode StatusCode { get; }
        public List<string> Messages { get; }
        public T? Value { get; }
        public bool HasValue => !EqualityComparer<T>.Default.Equals(Value, default);
        public string StringMessages => string.Join("\n", Messages);

        protected AzureDefaultCredentialResult(AzureDefaultCredentialResultCode statusCode, List<string> messages, T? value)
        {
            StatusCode = statusCode;
            Messages = messages ?? new List<string>(); // Ensure ErrorMessages is never null
            Value = value;
        }
    }

    public class AzureDefaultCredentialSuccessResult<T> : AzureDefaultCredentialResult<T>
    {
        public AzureDefaultCredentialSuccessResult(List<string> successMessages, T value) : base(AzureDefaultCredentialResultCode.OK, successMessages, value)
        {
        }
    }

    public class AzureDefaultCredentialEmptySuccessResult<T> : AzureDefaultCredentialResult<T>
    {
        public AzureDefaultCredentialEmptySuccessResult(List<string> successMessages) : base(AzureDefaultCredentialResultCode.NoContent, successMessages, default)
        {
        }
    }

    public class AzureDefaultCredentialErrorResult<T> : AzureDefaultCredentialResult<T>
    {
        public AzureDefaultCredentialErrorResult(List<string> errorMessages, AzureDefaultCredentialResultCode statusCode) : base(statusCode, errorMessages, default)
        {
        }
    }
    public enum AzureDefaultCredentialResultCode { OK, NoContent, Error, BadRequest }
}
