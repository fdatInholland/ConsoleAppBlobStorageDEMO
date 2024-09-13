using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace ConsoleAppBlobStorageDEMO
{
    internal class Program
    {
        private const string ConnectionString = "UseDevelopmentStorage=true";
        private const string ContainerName = "mycontainer";
        private const string BlobName = "example.txt";
        private const string FilePath = "example.txt";

        static async Task Main(string[] args)
        {
            // Create a BlobServiceClient to connect to the Azure Storage account
            var blobServiceClient = new BlobServiceClient(ConnectionString);

            // Create a container client
            var containerClient = blobServiceClient.GetBlobContainerClient(ContainerName);

            // Create the container if it does not exist
            await containerClient.CreateIfNotExistsAsync();

            // Upload a blob
            var blobClient = containerClient.GetBlobClient(BlobName);
            await UploadBlobAsync(blobClient, FilePath);

            // Download the blob
            await DownloadBlobAsync(blobClient, "downloaded_" + BlobName);

            // Delete the blob
            await DeleteBlobAsync(blobClient);

            Console.WriteLine("Blob operations completed.");
        }

        private static async Task UploadBlobAsync(BlobClient blobClient, string filePath)
        {
            Console.WriteLine($"Uploading to Blob storage as blob:\n\t{blobClient.Uri}\n");

            // Upload the file to the blob
            using (var fileStream = File.OpenRead(filePath))
            {
                await blobClient.UploadAsync(fileStream, overwrite: true);
            }

            Console.WriteLine("Upload completed.");
        }

        private static async Task DownloadBlobAsync(BlobClient blobClient, string downloadFilePath)
        {
            Console.WriteLine($"Downloading blob to:\n\t{downloadFilePath}\n");

            // Download the blob to a file
            BlobDownloadInfo download = await blobClient.DownloadAsync();
            using (var fileStream = File.OpenWrite(downloadFilePath))
            {
                await download.Content.CopyToAsync(fileStream);
            }

            Console.WriteLine("Download completed.");
        }

        private static async Task DeleteBlobAsync(BlobClient blobClient)
        {
            Console.WriteLine($"Deleting blob:\n\t{blobClient.Uri}\n");

            // Delete the blob
            await blobClient.DeleteIfExistsAsync();

            Console.WriteLine("Delete completed.");
        }
    }
}
