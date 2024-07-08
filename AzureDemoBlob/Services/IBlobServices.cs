using System.Reflection.Metadata;
using AzureDemoBlob.Models;

namespace AzureDemoBlob.Services
{
    public interface IBlobServices
    {
        Task<string> GetBlob(string name, string containerName);
        Task<List<string>> GetAllBlobs(string containerName);
        Task<List<Models.Blob>> GetAllBlobsWithUri(string containerName);
        Task<bool> UploadBlob(string name, IFormFile file, string containerName, Models.Blob blob);
        Task<bool> DeleteBlob(string name, string containerName);
    }
}
