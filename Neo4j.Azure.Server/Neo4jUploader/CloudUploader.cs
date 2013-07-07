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

      public CloudUploader() {
         Timeout = TimeSpan.FromMinutes(30);
      }

      public TimeSpan Timeout { get; set; }

      public void InitialiseCloudBlobClient(string protocol, string accountName, string accountKey) {
         var cloudStorageAccount = CloudStorageAccount.Parse(string.Format("DefaultEndpointsProtocol={0};AccountName={1};AccountKey={2}", protocol, accountName, accountKey));
         _cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
      }

      public void InitialiseLocalBlobClient() {
         var localStorageAccount = CloudStorageAccount.Parse("UseDevelopmentStorage=true");
         _cloudBlobClient = localStorageAccount.CreateCloudBlobClient();
      }

      public void Upload() {
         _cloudBlobClient.Timeout = Timeout;
         var container = _cloudBlobClient.GetContainerReference("neo4j");
         container.CreateIfNotExist();
         UploadBlobsToContainer(container);
      }

      private static void UploadBlobsToContainer(CloudBlobContainer container) {
         UploadBlob(container, "jre7.zip", "binaries\\jre7.zip");
         UploadBlob(container, "neo4j-community-1.8.2.zip", "binaries\\neo4j-community-1.8.2.zip");
      }

      private static void UploadBlob(CloudBlobContainer container, string blobName, string filename) {
         var blob = container.GetBlobReference(blobName);

         using (var fileStream = File.OpenRead(filename))
            blob.UploadFromStream(fileStream);
      }
   }
}
