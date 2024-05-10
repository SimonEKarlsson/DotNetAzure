namespace AzureServices.Services.Sharepoint
{
    public abstract class SharepointResult<T>
    {
        public SharepointResultCode StatusCode { get; }
        public List<string> Messages { get; }
        public T? Value { get; }
        public bool HasValue => !EqualityComparer<T>.Default.Equals(Value, default);
        public string StringMessages => string.Join("\n", Messages);

        protected SharepointResult(SharepointResultCode statusCode, List<string> messages, T? value)
        {
            StatusCode = statusCode;
            Messages = messages ?? new List<string>(); // Ensure ErrorMessages is never null
            Value = value;
        }
    }

    public class SharepointSuccessResult<T> : SharepointResult<T>
    {
        public SharepointSuccessResult(List<string> successMessages, T value) : base(SharepointResultCode.OK, successMessages, value)
        {
        }
    }

    public class SharepointEmptySuccessResult<T> : SharepointResult<T>
    {
        public SharepointEmptySuccessResult(List<string> successMessages) : base(SharepointResultCode.NoContent, successMessages, default)
        {
        }
    }

    public class SharepointErrorResult<T> : SharepointResult<T>
    {
        public SharepointErrorResult(List<string> errorMessages, SharepointResultCode statusCode) : base(statusCode, errorMessages, default)
        {
        }
    }
    public enum SharepointResultCode { OK, NoContent, Error, BadRequest }
}
