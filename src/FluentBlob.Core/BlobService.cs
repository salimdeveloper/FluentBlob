using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;

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
        /// <summary>
        /// Deletes blobitem in a container.
        /// </summary>
        /// <param name="fileName">BlobItem Name</param>
        /// <returns>Returns true if BlobItem is deleted</returns>
        public bool DeleteBlob(string fileName)
        {
            this._fileName = fileName ?? throw new ArgumentNullException(nameof(fileName));
            try
            {
                CloudBlobContainer container = GetBlobContainer();
                CloudBlob _blob = container.GetBlobReference(_fileName);
                return _blob.DeleteIfExists();
            }
            catch (StorageException _exception)
            {
                throw _exception;
            }
        }
        /// <summary>
        /// Delete all blob items in a container.
        /// </summary>
        public void DeleteAllBlobs()
        {
            BlobContinuationToken _continuationToken = null;
            try
            {
                CloudBlobContainer _blobContainer = GetBlobContainer();
                var _res = _blobContainer.ListBlobsSegmentedAsync(string.Empty, true, BlobListingDetails.All, new int?()
                            , _continuationToken, null, null);
                foreach (var blobItem in _res.Result.Results)
                {
                    CloudBlob _blobClient = _blobContainer.GetBlobReference(blobItem.Uri.ToString());
                    _blobClient.DeleteIfExists();
                }
            }
            catch (StorageException _exception)
            {
                throw _exception;
            }
        }
        /// <summary>
        /// Sets the blob item to upload.
        /// </summary>
        /// <param name="fileName">Name of the blob item.</param>
        /// <returns></returns>
        public IFileWriteActions UploadBlob(string fileName)
        {
            this._fileName = fileName;
            return this;
        }
        /// <summary>
        /// Sets the blob iitem to download
        /// </summary>
        /// <param name="fileName">Name of the blob item.</param>
        /// <returns></returns>
        public IFileReadActions DownloadBlob(string fileName)
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
            try
            {
                CloudBlobContainer cloudBlobContainer = GetBlobContainer();
                if (breakLease)
                {
                    cloudBlobContainer.BreakLeaseAsync(null);
                }
                var _returnValue = cloudBlobContainer.DeleteIfExists();
                return _returnValue;
            }
            catch (StorageException _storageException)
            {
                throw _storageException;
            }
        }
        /// <summary>
        /// Creates a new container in storage account.
        /// </summary>
        /// <returns>returns true if container is created.</returns>
        public bool CreateContainer()
        {
            CloudBlobContainer cloudBlobContainer = GetBlobContainer();
            try
            {
                return cloudBlobContainer.CreateIfNotExists();
            }
            catch (StorageException _exception)
            {
                throw _exception;
            }
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
        public IEnumerable<IListBlobItem> GetAllBlobItems()
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
            try
            {
                CloudBlobContainer _blobContainer = GetBlobContainer();
                CloudBlockBlob _cloudBlob = _blobContainer.GetBlockBlobReference(fileName);
                if (!_cloudBlob.Exists())
                {
                    throw new BlobNotFoundException();
                }
                string _sasBlobToken = GetSharedAccessToken(_cloudBlob, sharedAccessMinutes);
                return _cloudBlob.Uri + _sasBlobToken;
            }
            catch (StorageException _exception)
            {
                throw _exception;
            }
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
            try
            {
                var sasConstraints = new SharedAccessBlobPolicy();
                sasConstraints.SharedAccessStartTime = DateTime.UtcNow.AddMinutes(-1);
                sasConstraints.SharedAccessExpiryTime = DateTime.UtcNow.AddMinutes(sharedAccessMinutes);
                sasConstraints.Permissions = SharedAccessBlobPermissions.Read;

                var sasBlobToken = blob.GetSharedAccessSignature(sasConstraints);
                return sasBlobToken;
            }
            catch (StorageException _exception)
            {
                throw _exception;
            }
        }
        /// <summary>
        /// Gets Initialized Blob Container
        /// </summary>
        /// <returns>CloudBlobContainer</returns>
        private CloudBlobContainer GetBlobContainer()
        {
            try
            {
                CloudBlobClient _serviceClient = CloudStorageAccount.Parse(this._connectionString).CreateCloudBlobClient();
                var container = _serviceClient.GetContainerReference(this._containerName);
                return container;
            }
            catch (StorageException _exception)
            {
                throw _exception;
            }
        }
        private CloudBlobClient GetCloudBlobClient()
        {
            try
            {
                return CloudStorageAccount.Parse(this._connectionString).CreateCloudBlobClient();
            }
            catch (StorageException _exception)
            {
                throw _exception;
            }

        }




        #endregion

    }


}
