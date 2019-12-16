using System.IO;

namespace FluentBlob.Core
{
    /// <author>
    /// SALIM ALAM
    /// DATE: 12/10/2019
    /// https://github.com/salimdeveloper
    /// </author>
    public interface IFileReadActions
    {
        //void ToFile(string filePath);
        void ToStream(Stream stream);
    }
}