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
        IFileReadActions Download(string fileName);
        IFileWriteActions Upload(string fileName);
        string GetSharedUri(string fileName, int sharedAccessMinutes);
        void Delete(string fileName);
        IEnumerable<IListBlobItem> GetAllFiles();
    }
}