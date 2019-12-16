using Microsoft.Azure.Storage.Blob;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        Task Delete(string fileName);
        void DeleteAllBlobs();
        IEnumerable<IListBlobItem> GetAllFiles();
        bool DeleteContainer(bool breakLease);
        bool CreateContainer();
    }
}