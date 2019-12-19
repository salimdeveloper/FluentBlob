using FluentBlob.Core;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Xunit;

namespace FluentBlob.Test
{
    public class TestBlobs : IClassFixture<BlobStorageFixture>
    {
        private readonly string _conString = "UseDevelopmentStorage=true;";
        private BlobStorageFixture _fixture;
         

        public TestBlobs(BlobStorageFixture fixture)
        {
            _fixture = fixture;
        }
        [Fact]
        public void DeleteBlob_ForBlobDeleted_ReturnsTure()
        {
            string _containerName = "testcontainer"+ Guid.NewGuid().ToString().ToLower();
            string _blobName = "testfile" + Guid.NewGuid().ToString() + ".txt"; ;
            var container = Client().GetContainerReference(_containerName);
            container.CreateIfNotExistsAsync().Wait();
            CloudBlockBlob blob = container.GetBlockBlobReference(_blobName);
            blob.UploadTextAsync("Hello, World 2!").Wait();

            BlobService.Connect(_conString).Container(_containerName).DeleteBlob(_blobName);

            var _blobExists = container.GetBlockBlobReference(_blobName).Exists();
            Assert.False(_blobExists);
        }
        [Fact]
        public void UploadBlobFromStream_ForFileUploaded_ReturnsTrus()
        {
            // Create a local file in the ./data/ directory for uploading and downloading
            string _containerName = "testcontainer" + Guid.NewGuid().ToString().ToLower();
            string _localPath = "../../../data/";
            string _blobName = "testblob" + Guid.NewGuid().ToString() + ".txt";
            string _localFilePath = Path.Combine(_localPath, _blobName);
            // Write text to the file
            File.WriteAllTextAsync(_localFilePath, "Hello, World!").Wait();
            //Create a container
            var containerClient = Client().GetContainerReference(_containerName);
            containerClient.CreateIfNotExistsAsync().Wait();
            // Get a reference to a blob
            CloudBlockBlob blob = containerClient.GetBlockBlobReference(_blobName);

            // Open the file and upload its data
            using FileStream _uploadFileStream = File.OpenRead(_localFilePath);
            BlobService.Connect(_conString).Container(_containerName).UploadBlob(_blobName).FromStream(_uploadFileStream);
            _uploadFileStream.Close();

            var _blobExists = containerClient.GetBlockBlobReference(_blobName).Exists();

            Assert.True(_blobExists);

        }
        [Fact]
        public void DownloadBlobToStream_ForBlobDownloaded_ReturnsTrue()
        {
            string _containerName = "testcontainer" + Guid.NewGuid().ToString().ToLower();
            string _blobName = "testfile" + Guid.NewGuid().ToString() + ".txt"; ;
            var container = Client().GetContainerReference(_containerName);
            container.CreateIfNotExistsAsync().Wait();
            CloudBlockBlob blob = container.GetBlockBlobReference(_blobName);
            blob.UploadTextAsync("Hello, World downloaded test file!").Wait();

            //Prepare File
           
            string _localPath = "../../../data/";
            
            string _localFilePath = Path.Combine(_localPath, _blobName);

            using FileStream _filestream = File.OpenWrite(_localFilePath);
            BlobService.Connect(_conString).Container(_containerName).DownloadBlob(_blobName).ToStream(_filestream);

            Assert.True(File.Exists(_localFilePath));
            
            
        }
        [Fact]
        public void GetAllBlobItems_ForAllBlobs_ReturnsCount()
        {
            //Create no of blobs
            int _noOfBlobsToCreate = 7;
            string _containerName = "testcontainer" + Guid.NewGuid().ToString().ToLower();
            string _blobName = "testfile" + Guid.NewGuid().ToString() + ".txt"; 
            var container = Client().GetContainerReference(_containerName);
            container.CreateIfNotExistsAsync().Wait();
            for (int i = 0; i < _noOfBlobsToCreate; i++)
            {
                CloudBlockBlob blob = container.GetBlockBlobReference("testfile_" +i +Guid.NewGuid().ToString() + ".txt");
                blob.UploadTextAsync("blob").Wait();
            }
            //Read No of Blob Items 
            var _result =BlobService.Connect(_conString).Container(_containerName).GetAllBlobItems();
           Assert.Equal(_noOfBlobsToCreate, _result.ToList().Count());

        }
        [Fact]
        public void GetSharedUri_ForABlob_ReturnsTrue()
        {
            string _containerName = "testcontainer" + Guid.NewGuid().ToString().ToLower();
            string _blobName = "testfile" + Guid.NewGuid().ToString() + ".txt"; ;
            var container = Client().GetContainerReference(_containerName);
            container.CreateIfNotExistsAsync().Wait();
            CloudBlockBlob blob = container.GetBlockBlobReference(_blobName);
            blob.UploadTextAsync("Hello, World downloaded test file!").Wait();

            var _sharedAccessUri = BlobService.Connect(_conString).Container(_containerName).GetSharedUri("abc.txt",10);
            Assert.NotNull(_sharedAccessUri);
        }
        static CloudBlobClient Client() => CloudStorageAccount.Parse("UseDevelopmentStorage=true;").CreateCloudBlobClient();
    }
}
