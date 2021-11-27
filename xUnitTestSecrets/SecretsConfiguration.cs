

using System;
using System.IO;
using Xunit;
using xUnitTestSecrets.SecretProviders;

namespace xUnitTestSecrets
{
    public class SecretsConfiguration : IDisposable
    {
        private ISecretProvider _provider;
        public SecretsConfiguration()
        {
            // ... initialize data in the test database ...
        }

        public void Dispose()
        {
            // ... clean up test data from the database ...
        }

        public void DefaultConfiguration()
        {
            if (_provider == null)
            {
                var basePath = AppDomain.CurrentDomain.BaseDirectory;
                var cfgFilePath = Path.Combine(basePath, "secrets.json");
                var fileProvider = new JsonFileProvider(cfgFilePath);
                ConfigureProvider(fileProvider);
            }
        }

        public void ConfigureProvider(ISecretProvider provider)
        {
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));
        }

        public string GetSecret(string key)
        {
            return _provider.GetSecret(key);
        }
    }

    [CollectionDefinition("Secrets")]
    public class SecretsCollection : ICollectionFixture<SecretsConfiguration>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }

}
