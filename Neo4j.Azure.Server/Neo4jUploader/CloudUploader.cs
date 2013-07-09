using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;

namespace Neo4jUploader {
   public class CloudUploader {
      private CloudBlobClient _cloudBlobClient;
      private string _accountKey;
      private string _accountName;

      public CloudUploader() {
         Timeout = TimeSpan.FromMinutes(30);
      }

      public TimeSpan Timeout { get; set; }

      public void InitialiseCloudBlobClient(string protocol, string accountName, string accountKey) {

         if (accountName == _accountName && accountKey == _accountKey) {}
         else {
            _accountName = accountName;
            _accountKey = accountKey;
            var cloudStorageAccount = CloudStorageAccount.Parse(string.Format("DefaultEndpointsProtocol={0};AccountName={1};AccountKey={2}", protocol, accountName, accountKey));
            _cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
         }
      }

      public void InitialiseLocalBlobClient() {
         var localStorageAccount = CloudStorageAccount.Parse("UseDevelopmentStorage=true");
         _cloudBlobClient = localStorageAccount.CreateCloudBlobClient();
      }

      public void Upload(string storageName, string pathToFile) {
         _cloudBlobClient.Timeout = Timeout;
         var container = _cloudBlobClient.GetContainerReference("neo4j");
         container.CreateIfNotExist();
         UploadBlobsToContainer(container, storageName, pathToFile);
      }

      private static void UploadBlobsToContainer(CloudBlobContainer container, string javaStorageName, string javaFilePath) {
         UploadBlob(container, javaStorageName, javaFilePath);
      }

      private static void UploadBlob(CloudBlobContainer container, string blobName, string filename) {
         var blob = container.GetBlobReference(blobName);

         using (var fileStream = File.OpenRead(filename))
            blob.UploadFromStream(fileStream);
      }
   }
}
