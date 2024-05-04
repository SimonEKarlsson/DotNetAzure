using Azure.Core;
using Azure.Identity;

namespace AzureServices.Services.AzureDefaultCredential
{
    public class AzureDefaultCredentialService : IAzureDefaultCredentialService
    {
        //Azure.Identity 1.11.2
        private AccessToken AccessToken;

        public AzureDefaultCredentialService()
        {
            AccessToken = new AccessToken("", DateTimeOffset.MinValue);
        }

        private void UpdateToken()
        {
            try
            {
                var credential = new DefaultAzureCredential();
                var token = credential.GetToken(new TokenRequestContext(new[] { "https://graph.microsoft.com/.default" })); //change to correct scope as needed
                AccessToken = token;
            }
            catch
            {
                AccessToken = new AccessToken("", DateTimeOffset.MinValue);
            }
        }
        
        private bool IsTokenExpired(int expirationThresholdSeconds = 60)
        {
            return string.IsNullOrEmpty(AccessToken.Token) || AccessToken.ExpiresOn < DateTimeOffset.UtcNow.AddSeconds(expirationThresholdSeconds);
        }

        public AzureDefaultCredentialResult<string?> GetToken()
        {
            try
            {
                //check if token is expired
                if (IsTokenExpired())
                {
                    UpdateToken();
                }

                //if token is still null or expired, return error
                if (IsTokenExpired())
                {
                    return new AzureDefaultCredentialErrorResult<string?>(new List<string> { "Error getting Azure Default Credential" }, AzureDefaultCredentialResultCode.Error);
                }

                return new AzureDefaultCredentialSuccessResult<string?>(new List<string> { }, AccessToken.Token);
            }
            catch (Exception e)
            {
                return new AzureDefaultCredentialErrorResult<string?>(new List<string> { "Error getting Azure Default Credential", e.Message }, AzureDefaultCredentialResultCode.Error);
            }
        }
    }
}
