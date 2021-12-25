using R.Systems.Shared.Core.Interfaces;
using System.IO;
using System.Reflection;

namespace R.Systems.Shared.FunctionalTests.Models;

internal class EmbeddedRsaKeys : IRsaKeys
{
    public EmbeddedRsaKeys()
    {
        ResourceLoader resourceLoader = new();
        Assembly assembly = GetType().Assembly;
        string publicKeyFileName = "public.pem";
        PublicKey = resourceLoader.GetEmbeddedResourceString(assembly, publicKeyFileName);
        if (PublicKey == null)
        {
            throw new FileNotFoundException($"RSA public key file ({publicKeyFileName}) doesn't exist.");
        }
    }

    public string? PublicKey { get; init; }

    public string? PrivateKey { get; init; }
}
