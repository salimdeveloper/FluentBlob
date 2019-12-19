using Microsoft.Azure.Storage;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace FluentBlob.Core
{
    class BlobNotFoundException : StorageException
    {
        public BlobNotFoundException()
        {
        }

        public BlobNotFoundException(string message) : base("Blob Not Found !")
        {
        }

        public BlobNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public BlobNotFoundException(RequestResult res, string message, Exception inner) : base(res, message, inner)
        {
        }

        protected BlobNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
