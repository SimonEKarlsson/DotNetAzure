using System.Net;

namespace AzureServices.Services.ApplicationInsights
{
    public abstract class AppInsightsResult<T>
    {
        public AppInsightsResultCode StatusCode { get; }
        public List<string> Messages { get; }
        public T Value { get; }
        public bool HasValue => !EqualityComparer<T>.Default.Equals(Value, default(T));

        protected AppInsightsResult(AppInsightsResultCode statusCode, List<string> messages, T value)
        {
            StatusCode = statusCode;
            Messages = messages ?? new List<string>(); // Ensure ErrorMessages is never null
            Value = value;
        }
    }

    public class AppInsightsSuccessResult<T> : AppInsightsResult<T>
    {
        public AppInsightsSuccessResult(List<string> successMessages, T value) : base(AppInsightsResultCode.OK, successMessages, value)
        {
        }
    }

    public class AppInsightsEmptySuccessResult<T> : AppInsightsResult<T>
    {
        public AppInsightsEmptySuccessResult(List<string> successMessages) : base(AppInsightsResultCode.NoContent, successMessages, default(T) ?? throw new Exception($"value {typeof(T)} is null"))
        {
        }
    }

    public class AppInsightsErrorResult<T> : AppInsightsResult<T>
    {
        public AppInsightsErrorResult(List<string> errorMessages, AppInsightsResultCode statusCode) : base(statusCode, errorMessages, default(T) ?? throw new Exception($"value {typeof(T)} is null"))
        {
        }
    }
    public enum AppInsightsResultCode { OK, NoContent, Error }
}
