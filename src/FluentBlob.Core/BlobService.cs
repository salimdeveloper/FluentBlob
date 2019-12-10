using System;
using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage.Blob;

namespace FluentBlob.Core
{
    public sealed class BlobService:IBlobActions,IContainerActions
    {
        private readonly string _connectionString;
        private object _containerName;

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
        public void Delete(string fileName)
        {
            throw new NotImplementedException();
        }
        public IFileReadActions Download(string fileName)
        {
            throw new NotImplementedException();
        }
        public IEnumerable<IListBlobItem> GetAllFiles()
        {
            throw new NotImplementedException();
        }
        public string GetSharedUrl(string fileName, int sharedAccessMinutes)
        {
            throw new NotImplementedException();
        }
        public IFileWriteActions Upload(string fileName)
        {
            throw new NotImplementedException();
        }
    }


}
