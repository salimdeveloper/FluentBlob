using System;

namespace FluentBlob.Core
{
    public sealed class BlobService
    {
        private readonly string _connectionString;

        public BlobService(string connectionString) => 
            this._connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
      
    }


}
