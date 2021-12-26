using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace R.Systems.Shared.FunctionalTests.Tests.EntityTests;

public class DeleteEntityTests : EntityControllerTests
{
    [Fact]
    public async Task DeleteEntity_WithoutAuthenticationToken_Unauthorized()
    {
        long entityId = 1;
        (HttpStatusCode httpStatusCode, _) = await RequestService.SendDeleteAsync<object>(
            $"{EntitiesUrl}/{entityId}",
            HttpClient
        );

        Assert.Equal(HttpStatusCode.Unauthorized, httpStatusCode);
    }

    [Fact]
    public async Task DeleteEntity_UserWithoutRoleAdmin_Forbidden()
    {
        string accessToken = AuthenticatorService.GenerateAccessToken(
            userId: 1,
            userEmail: "user@lukaszrydzkowski.pl",
            userRolesKeys: new List<string>() { "user" },
            privateKeyPem: RsaKeysService.GetPrivateKey()
        );
        long entityId = 1;

        (HttpStatusCode httpStatusCode, _) = await RequestService.SendDeleteAsync<object>(
            $"{EntitiesUrl}/{entityId}",
            HttpClient,
            accessToken
        );

        Assert.Equal(HttpStatusCode.Forbidden, httpStatusCode);
    }

    [Fact]
    public async Task DeleteEntity_CorrectData_ReturnsOk()
    {
        string accessToken = AuthenticatorService.GenerateAccessToken(
            userId: 1,
            userEmail: "admin@lukaszrydzkowski.pl",
            userRolesKeys: new List<string>() { "admin" },
            privateKeyPem: RsaKeysService.GetPrivateKey()
        );
        long entityId = 1;

        (HttpStatusCode httpStatusCode, _) = await RequestService.SendDeleteAsync<object>(
            $"{EntitiesUrl}/{entityId}",
            HttpClient,
            accessToken
        );

        Assert.Equal(HttpStatusCode.OK, httpStatusCode);
    }
}
