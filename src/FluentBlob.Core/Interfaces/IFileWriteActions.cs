using System.IO;

namespace FluentBlob.Core
{
    /// <author>
    /// SALIM ALAM
    /// DATE: 12/10/2019
    /// https://github.com/salimdeveloper
    /// </author>
    public interface IFileWriteActions
    {
        void FromFile(string filePath);
        void FromStream(Stream stream);
    }
}