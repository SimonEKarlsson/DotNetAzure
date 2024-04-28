using AzureServices.Services.StorageAccount;
using Microsoft.AspNetCore.Components.Forms;

namespace DotNetAzure.UI.Pages
{
    public partial class StorageAccountUI
    {
        //StorageAccountService StorageAccountService { get; set; }
        private readonly long maxFileSize = 500 * 1024 * 1024; // 500MB


        private async Task BtnCreateContainer()
        {
            await SA.CreateContainerAsync();
        }

        private async Task BtnDownload()
        {
            await SA.ReadFileAsync("Az-204 short.docx");
        }

        private async Task BtnDelete()
        {
            await SA.DeleteFileAsync("Az-204 short.docx");
        }

        private async Task BtnGetFiles()
        {
            await SA.ListContainerFiles();
        }

        private async Task HandleFileUpload(InputFileChangeEventArgs e)
        {
            var uploadFailed = false;

            foreach (var file in e.GetMultipleFiles())
            {
                if (file != null || file!.Size != 0)
                {
                    await SA.UploadFile(file.Name, file.OpenReadStream(maxFileSize), true);
                    file.OpenReadStream().Close();
                }
                else
                {
                    uploadFailed = true;
                }
            }
            if (!uploadFailed)
            {
            }
        }
    }
}
