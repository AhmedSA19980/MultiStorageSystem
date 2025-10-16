using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlobStorage.Core.Interfaces.Factory
{
    public  interface IObjectStorageProviderFactory
    {
        IObjectStorage GetProvider(string providerName);
        
    }
}
