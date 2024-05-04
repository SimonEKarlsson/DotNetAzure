namespace AzureServices.Services.OAuth2Token
{
    public interface IOAuth2TokenService
    {
        Task<OAuth2TokenResult<string?>> GetTokenAsync();
        Task<OAuth2TokenResult<bool>> CheckRole(string role);
    }
}
