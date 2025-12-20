using Azure.Identity;
using Azure.Storage.Blobs;

string containerName = "test101";
string fileName = "test.pdf";
string path = @"./test.pdf";

Console.WriteLine("started");

string tenantId = "";
string clientId = "";
string secret = "";
string storageAccountName = "appstore55455344243";


string blobUri = $"https://{storageAccountName}.blob.core.windows.net/{containerName}/{fileName}";


ClientSecretCredential clientSecretCredential = new ClientSecretCredential(tenantId, clientId, secret);
BlobClient blobClient = new BlobClient(new Uri(blobUri), clientSecretCredential);

await blobClient.DownloadToAsync(path);



Console.WriteLine("done!");
