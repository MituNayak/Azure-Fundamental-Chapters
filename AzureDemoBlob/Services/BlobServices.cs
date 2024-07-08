using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using System.Reflection.Metadata;
using AzureDemoBlob.Models;

namespace AzureDemoBlob.Services
{
    public class BlobServices : IBlobServices
    {
        private readonly BlobServiceClient _blobClient;
        public BlobServices(BlobServiceClient blobClient)
        {
           _blobClient = blobClient;
        }
        public async Task<bool> DeleteBlob(string name, string containerName)
        {
            BlobContainerClient blobContainerClient = _blobClient.GetBlobContainerClient(containerName);

            var blobClient = blobContainerClient.GetBlobClient(name);

            return await blobClient.DeleteIfExistsAsync();
        }

        public async Task<List<string>> GetAllBlobs(string containerName)
        {
            BlobContainerClient blobContainerClient = _blobClient.GetBlobContainerClient(containerName);
            var blobs = blobContainerClient.GetBlobsAsync();

            var blobString = new List<string>();

            await foreach (var item in blobs)
            {
                blobString.Add(item.Name);
            }

            return blobString;
        }

        public async Task<List<Models.Blob>> GetAllBlobsWithUri(string containerName)
        {
            BlobContainerClient blobContainerClient = _blobClient.GetBlobContainerClient(containerName);
            var blobs = blobContainerClient.GetBlobsAsync();
            var blobList = new List<Models.Blob>();
            string sasContainerSignature = "";

            if (blobContainerClient.CanGenerateSasUri)
            {
                BlobSasBuilder sasBuilder = new()
                {
                    BlobContainerName = blobContainerClient.Name,
                    Resource = "c",
                    ExpiresOn = DateTimeOffset.UtcNow.AddHours(1)
                };

                sasBuilder.SetPermissions(BlobSasPermissions.Read);

                sasContainerSignature = blobContainerClient.GenerateSasUri(sasBuilder).AbsoluteUri.Split('?')[1].ToString();
            }



            await foreach (var item in blobs)
            {
                var blobClient = blobContainerClient.GetBlobClient(item.Name);
                Models.Blob blobIndividual = new()
                {
                    Uri = blobClient.Uri.AbsoluteUri + "?" + sasContainerSignature
                };

                     BlobProperties properties = await blobClient.GetPropertiesAsync();
                if (properties.Metadata.ContainsKey("title"))
                {
                    blobIndividual.Title = properties.Metadata["title"];
                }
                if (properties.Metadata.ContainsKey("comment"))
                {
                    blobIndividual.Comment = properties.Metadata["comment"];
                }
                blobList.Add(blobIndividual);
            }

            return blobList;
        }

        public async Task<string> GetBlob(string name, string containerName)
        {
            BlobContainerClient blobContainerClient = _blobClient.GetBlobContainerClient(containerName);

            var blobClient = blobContainerClient.GetBlobClient(name);

            return blobClient.Uri.AbsoluteUri;
        }

        //public async Task<bool> UploadBlob(string name, IFormFile file, string containerName, Models.Blob blob)
        //{
        //    BlobContainerClient blobContainerClient = _blobClient.GetBlobContainerClient(containerName);

        //    var blobClient = blobContainerClient.GetBlobClient(name);

        //    var httpHeaders = new BlobHttpHeaders()
        //    {
        //        ContentType = file.ContentType
        //    };

        //    IDictionary<string, string> metadata =
        //     new Dictionary<string, string>();

        //    metadata.Add("title", blob.Title);
        //    metadata["comment"] = blob.Comment;

        //    var result = await blobClient.UploadAsync(file.OpenReadStream(), httpHeaders, metadata);

        //    //metadata.Remove("title");

        //    //await blobClient.SetMetadataAsync(metadata);

        //    if (result != null)
        //    {
        //        return true;
        //    }
        //    return false;
        //}

        public async Task<bool> UploadBlob(string name, IFormFile file, string containerName,Models.Blob blob)
        {
            BlobContainerClient blobContainerClient = _blobClient.GetBlobContainerClient(containerName);

            var blobClient = blobContainerClient.GetBlobClient(name);

            var httpHeaders = new BlobHttpHeaders()
            {
                ContentType = file.ContentType
            };

            IDictionary<string, string> metadata =
             new Dictionary<string, string>();

            metadata.Add("title", blob.Title);
            metadata["comment"] = blob.Comment;

            var result = await blobClient.UploadAsync(file.OpenReadStream(), httpHeaders, metadata);

            //metadata.Remove("title");

            //await blobClient.SetMetadataAsync(metadata);

            if (result != null)
            {
                return true;
            }
            return false;
        }

       
    }
}
