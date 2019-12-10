using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Text;

namespace FluentBlob.Core
{
    public interface IContainerActions
    {
        IFileReadActions Download(string fileName);
        string GetSharedUrl(string fileName, int sharedAccessMinutes);

        IEnumerable<IListBlobItem> GetAllFiles();
    }
}
