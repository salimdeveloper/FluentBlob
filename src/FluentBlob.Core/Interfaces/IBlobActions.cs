﻿using Microsoft.Azure.Storage.Blob;
using System.Collections.Generic;

namespace FluentBlob.Core
{
    /// <author>
    /// SALIM ALAM
    /// DATE: 12/10/2019
    /// https://github.com/salimdeveloper
    /// </author>
    public interface IBlobActions
    {
        IContainerActions Container(string containerName);
        IEnumerable<CloudBlobContainer> GetAllContainers();

    }
}
