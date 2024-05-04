namespace AzureServices.Services.AzureDefaultCredential
{
    public interface IAzureDefaultCredentialService
    {
        AzureDefaultCredentialResult<string?> GetToken();
    }
}
