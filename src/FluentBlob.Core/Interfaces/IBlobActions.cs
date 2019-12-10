using System;
using System.Collections.Generic;
using System.Text;

namespace FluentBlob.Core
{
    public interface IBlobActions
    {
        IContainerActions GetContainer(string containerName);
        IContainerActions SetContainer(string containerName);
    }
}
