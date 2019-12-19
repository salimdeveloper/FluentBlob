using Microsoft.Azure.Storage.Blob;
using System.Collections.Generic;

namespace FluentBlob.Core
{
    /// <author>
    /// SALIM ALAM
    /// DATE: 12/10/2019
    /// https://github.com/salimdeveloper
    /// </author>
    public interface IContainerActions
    {
        IFileReadActions DownloadBlob(string fileName);
        IFileWriteActions UploadBlob(string fileName);
        string GetSharedUri(string fileName, int sharedAccessMinutes);
        bool DeleteBlob(string fileName);
        void DeleteAllBlobs();
        IEnumerable<IListBlobItem> GetAllBlobItems();
        bool DeleteContainer(bool breakLease);
        bool CreateContainer();
    }
}