using System;

namespace FluentBlob.Core
{
    public sealed class BlobService:IBlobActions
    {
        private readonly string _connectionString;

        public BlobService(string connectionString) => 
            this._connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));

        public static IBlobActions Connect(string connectionString) =>
           new BlobService(connectionString);

        public IContainerActions GetContainer(string containerName)
        {
            throw new NotImplementedException();
        }

        public IContainerActions SetContainer(string containerName)
        {
            throw new NotImplementedException();
        }
    }


}
