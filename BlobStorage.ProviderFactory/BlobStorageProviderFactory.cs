using BlobStorage.Core.Interfaces;
using BlobStorage.Core.Interfaces.Factory;

namespace BlobStorage.ProviderFactory
{

    public class ProviderFactory : IObjectStorageProviderFactory
    {
        private Dictionary<string, IObjectStorage> _providers;
        private readonly IObjectStorage _serviceProvider; 

        public ProviderFactory(IEnumerable<IObjectStorage> providers, IObjectStorage serviceProvider )
        {

            _providers = providers.ToDictionary(p => p.Name, StringComparer.OrdinalIgnoreCase);
            _serviceProvider = serviceProvider;
        }
        public IObjectStorage GetProvider(string providerName)
        {
           
            if (_providers.TryGetValue(providerName, out var provider))
            {
                return provider;
            }

            // fallback to SQL if not found
            return _providers["SqlDatabase"];
        
        }

       
    }
}
