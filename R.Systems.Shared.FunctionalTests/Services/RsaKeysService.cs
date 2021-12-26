using System.IO;
using System.Reflection;

namespace R.Systems.Shared.FunctionalTests.Services;

public class RsaKeysService
{
    public string GetPrivateKey()
    {
        string privateKeyFileName = "private.pem";
        ResourceLoader resourceLoader = new();
        Assembly assembly = GetType().Assembly;
        string privateKey = resourceLoader.GetEmbeddedResourceString(assembly, privateKeyFileName);
        if (privateKey == null)
        {
            throw new FileNotFoundException($"RSA private key file ({privateKeyFileName}) doesn't exist.");
        }
        return privateKey;
    }
}
