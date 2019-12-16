using System;
using System.Collections.Generic;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Azure.Storage;
using System.IO;
using System.Threading.Tasks;

namespace FluentBlob.Core
{
    /// <author>
    /// SALIM ALAM
    /// DATE: 12/10/2019
    /// https://github.com/salimdeveloper
    /// </author>
    public sealed class BlobService : IBlobActions, IContainerActions, IFileReadActions, IFileWriteActions
    {
        private readonly string _connectionString;
        private string _containerName;
        private string _fileName;

        public BlobService(string connectionString) =>
            this._connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));

        public static IBlobActions Connect(string connectionString) =>
           new BlobService(connectionString);
        public IContainerActions Container(string containerName)
        {
            this._containerName = containerName ??
                                  throw new ArgumentNullException(nameof(containerName));
            return this;
        }
        public async Task Delete(string fileName)
        {
            this._fileName = fileName ?? throw new ArgumentNullException(nameof(fileName));
            CloudBlobContainer container = GetBlobContainer();
            CloudBlob _blob = container.GetBlobReference(_fileName);
            await _blob.DeleteIfExistsAsync();
        }
        public IFileWriteActions Upload(string fileName)
        {
            this._fileName = fileName;
            return this;
        }
        public IFileReadActions Download(string fileName)
        {
            this._fileName = fileName;
            return this;
        }
        private void FromFile(string filePath)
        {
            throw new NotImplementedException();
        }

        public void FromStream(Stream stream)
        {
            CloudBlobContainer cloudBlobContainer = GetBlobContainer();
            CloudBlockBlob blockBlob = cloudBlobContainer.GetBlockBlobReference(this._fileName);
            blockBlob.UploadFromStream(stream);
        }
        /// <summary>
        /// Gets all containers in a storage account.
        /// </summary>
        /// <returns>Returns IEnumerable collection of CloudBlobContainer</returns>
        public IEnumerable<CloudBlobContainer> GetAllContainers()
        {
            BlobContinuationToken continuationToken = null;
            try
            {
                var _cloudBlobClient = GetCloudBlobClient();
                return _cloudBlobClient.ListContainersSegmentedAsync(continuationToken).Result.Results;
               
            }
            catch (StorageException _exxception)
            {

                throw _exxception;
            }
        }
        /// <summary>
        /// Deletes a container
        /// </summary>
        /// <param name="breakLease">Pass True if you want to break lease of container</param>
        /// <returns>Returns true of container deleted successfully</returns>
        public bool DeleteContainer(bool breakLease)
        {
            try { 
            CloudBlobContainer cloudBlobContainer = GetBlobContainer();
            if (breakLease)
            {
                cloudBlobContainer.BreakLeaseAsync(null);
            }
            var _returnValue = cloudBlobContainer.DeleteIfExists();
            return _returnValue;
            }
            catch(StorageException _storageException)
            {
                throw _storageException;
            }
        }

        public void CreateContainer()
        {
            CloudBlobContainer cloudBlobContainer = GetBlobContainer();
            cloudBlobContainer.CreateIfNotExists();
        }
        private void ToFile(string filePath)
        {
            throw new NotImplementedException();
        }

        public void ToStream(Stream stream)
        {
            CloudBlobContainer cloudBlobContainer = GetBlobContainer();
            CloudBlockBlob blockBlob = cloudBlobContainer.GetBlockBlobReference(this._fileName);
            blockBlob.DownloadToStream(stream);
        }
        /// <summary>
        /// Gets all blob items in a container
        /// </summary>
        /// <returns>IEnumerable<IListBlobItem></returns>
        public IEnumerable<IListBlobItem> GetAllFiles()
        {
            CloudBlobContainer _blobContainer = GetBlobContainer();
            BlobContinuationToken _continuationToken = null;

            {
                do
                {
                    var _response = _blobContainer.ListBlobsSegmentedAsync(string.Empty, true, BlobListingDetails.All, new int?()
                        , _continuationToken, null, null);
                    foreach (var blob in _response.Result.Results)
                    {
                        yield return blob;
                    }
                } while (_continuationToken != null);
            };

        }


        /// <summary>
        /// Gets blob uri with shared access token.
        /// </summary>
        /// <param name="fileName">String</param>
        /// <param name="sharedAccessMinutes">int</param>
        /// <returns></returns>
        public string GetSharedUri(string fileName, int sharedAccessMinutes)
        {
            CloudBlobContainer _blobContainer = GetBlobContainer();
            CloudBlockBlob _cloudBlob = _blobContainer.GetBlockBlobReference(fileName);
            string _sasBlobToken = GetSharedAccessToken(_cloudBlob, sharedAccessMinutes);
            return _cloudBlob.Uri + _sasBlobToken;
        }

        #region Helper Methods
        /// <summary>
        /// Initializes SharedAccess,Sets Access Time and Permission
        /// </summary>
        /// <param name="blob">CloudBlockBlob</param>
        /// <param name="sharedAccessMinutes">int</param>
        /// <returns>Blob Token</returns>
        private static string GetSharedAccessToken(CloudBlockBlob blob, int sharedAccessMinutes)
        {
            var sasConstraints = new SharedAccessBlobPolicy();
            sasConstraints.SharedAccessStartTime = DateTime.UtcNow.AddMinutes(-1);
            sasConstraints.SharedAccessExpiryTime = DateTime.UtcNow.AddMinutes(sharedAccessMinutes);
            sasConstraints.Permissions = SharedAccessBlobPermissions.Read;

            var sasBlobToken = blob.GetSharedAccessSignature(sasConstraints);
            return sasBlobToken;
        }
        /// <summary>
        /// Gets Initialized Blob Container
        /// </summary>
        /// <returns>CloudBlobContainer</returns>
        private CloudBlobContainer GetBlobContainer()
        {
            CloudBlobClient _serviceClient = CloudStorageAccount.Parse(this._connectionString).CreateCloudBlobClient();
            var container = _serviceClient.GetContainerReference(this._containerName);
            return container;
        }
        private CloudBlobClient GetCloudBlobClient()
        {
            return CloudStorageAccount.Parse(this._connectionString).CreateCloudBlobClient();
        }




        #endregion

    }


}
