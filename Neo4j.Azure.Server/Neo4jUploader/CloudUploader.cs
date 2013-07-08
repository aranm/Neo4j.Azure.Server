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

      public void Upload(string javaStorageName, string binariesJre7Zip, string noe4JStorageName, string binariesNeo4jCommunityZip) {
         _cloudBlobClient.Timeout = Timeout;
         var container = _cloudBlobClient.GetContainerReference("neo4j");
         container.CreateIfNotExist();
         UploadBlobsToContainer(container, javaStorageName, binariesJre7Zip, noe4JStorageName, binariesNeo4jCommunityZip);
      }

      private static void UploadBlobsToContainer(CloudBlobContainer container, string javaStorageName, string javaFilePath, string noe4JStorageName, string neo4JFilePath) {
         UploadBlob(container, javaStorageName, javaFilePath);
         UploadBlob(container, noe4JStorageName, neo4JFilePath);
      }

      private static void UploadBlob(CloudBlobContainer container, string blobName, string filename) {
         var blob = container.GetBlobReference(blobName);

         using (var fileStream = File.OpenRead(filename))
            blob.UploadFromStream(fileStream);
      }
   }
}
