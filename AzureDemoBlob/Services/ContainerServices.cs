using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;

namespace AzureDemoBlob.Services
{
    public class ContainerServices : IContainerServices
    {

        private readonly BlobServiceClient _blobClient;

        public ContainerServices(BlobServiceClient blobClient)
        {
            _blobClient = blobClient;
        }
        public async Task CreateContainer(string containerName)
        {
            BlobContainerClient blobContainerClient = _blobClient.GetBlobContainerClient(containerName);
            await blobContainerClient.CreateIfNotExistsAsync(PublicAccessType.BlobContainer);

        }

        public async Task DeleteContainer(string containerName)
        {
            BlobContainerClient blobContainerClient = _blobClient.GetBlobContainerClient(containerName);
            await blobContainerClient.DeleteIfExistsAsync();

        }

        public async Task<List<string>> GetAllContainer()
        {
            List<string> containerName = new();

            await foreach (BlobContainerItem blobkContainerItem in _blobClient.GetBlobContainersAsync())
            {
                containerName.Add(blobkContainerItem.Name);
            }

            return containerName;
        }

        public Task<List<string>> GetAllContainerAndBlobs()
        {
            throw new NotImplementedException();
        }


    }
}
