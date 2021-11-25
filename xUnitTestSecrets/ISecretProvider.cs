using System;
namespace xUnitTestSecrets
{
    public interface ISecretProvider
    {
        string GetSecret(string keyValue);
    }
}
