using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace R.Systems.Shared.FunctionalTests.Services;

internal class AuthenticatorService
{
    public string GenerateAccessToken(
        long userId,
        string userEmail,
        List<string> userRolesKeys,
        string privateKeyPem,
        double lifetimeInMinutes = 10)
    {
        IDictionary<string, object> claims = GenerateUsersClaims(userId, userEmail, userRolesKeys);
        DateTime? expires = DateTime.UtcNow.AddMinutes(lifetimeInMinutes);

        using RSA rsa = RSA.Create();
        rsa.ImportFromPem(privateKeyPem.ToCharArray());
        SigningCredentials signingCredentials = new(new RsaSecurityKey(rsa), SecurityAlgorithms.RsaSha384)
        {
            CryptoProviderFactory = new CryptoProviderFactory { CacheSignatureProviders = false }
        };

        JwtSecurityTokenHandler tokenHandler = new();
        SecurityTokenDescriptor tokenDescriptor = new()
        {
            Claims = claims,
            Expires = expires,
            SigningCredentials = signingCredentials
        };
        SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    private Dictionary<string, object> GenerateUsersClaims(long userId, string userEmail, List<string> userRolesKeys)
    {
        Dictionary<string, object> claims = new()
        {
            { ClaimTypes.NameIdentifier, userId },
            { ClaimTypes.Email, userEmail },
            { ClaimTypes.Role, userRolesKeys }
        };
        return claims;
    }
}
