using Azure.Identity;
using Azure.Storage.Blobs;


string containerName="data";
string fileName="script01.ps1";
string path=@"C:\tmp4\script01.ps1";
string tenantId="38dbefc3-d57f-4955-b62c-1406e16a4ea8";
string clientId="72ee1803-08cf-4c19-a334-8405e09a242c";
string secret="ywo8Q~iDGv1b1.Uk_CkXbLUw2G4YNLbWUo5s4b1T";
string storageAccountName="appstore55455344243";
string blobUri=$"https://{storageAccountName}.blob.core.windows.net/{containerName}/{fileName}";


ClientSecretCredential clientSecretCredential=new ClientSecretCredential(tenantId,clientId,secret);
BlobClient blobClient= new BlobClient(new Uri(blobUri),clientSecretCredential);

await blobClient.DownloadToAsync(path);


