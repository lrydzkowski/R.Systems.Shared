using R.Systems.Shared.FunctionalTests.Services;
using R.Systems.Shared.WebApiTest.Models;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace R.Systems.Shared.FunctionalTests.Tests.EntityTests;

public class GetEntitiesTests : EntityControllerTests
{
    [Fact]
    public async Task GetEntities_WithoutAuthenticationToken_Unauthorized()
    {
        (HttpStatusCode httpStatusCode, List<Entity>? entities) = await RequestService.SendGetAsync<List<Entity>>(
            EntitiesUrl,
            HttpClient
        );

        Assert.Equal(HttpStatusCode.Unauthorized, httpStatusCode);
        Assert.Null(entities);
    }

    [Fact]
    public async Task GetEntities_UserWithoutRoleAdmin_Forbidden()
    {
        string accessToken = AuthenticatorService.GenerateAccessToken(
            userId: 1,
            userEmail: "user@lukaszrydzkowski.pl",
            userRolesKeys: new List<string>() { "user" },
            privateKeyPem: RsaKeysService.GetPrivateKey()
        );

        (HttpStatusCode httpStatusCode, List<Entity>? entities) = await RequestService.SendGetAsync<List<Entity>>(
            EntitiesUrl,
            HttpClient,
            accessToken
        );

        Assert.Equal(HttpStatusCode.Forbidden, httpStatusCode);
        Assert.Null(entities);
    }

    [Fact]
    public async Task GetEntities_CorrectData_ReturnsEntities()
    {
        string accessToken = AuthenticatorService.GenerateAccessToken(
            userId: 1,
            userEmail: "admin@lukaszrydzkowski.pl",
            userRolesKeys: new List<string>() { "admin" },
            privateKeyPem: RsaKeysService.GetPrivateKey()
        );

        (HttpStatusCode httpStatusCode, List<Entity>? entities) = await RequestService.SendGetAsync<List<Entity>>(
            EntitiesUrl,
            HttpClient,
            accessToken
        );

        Assert.Equal(HttpStatusCode.OK, httpStatusCode);
        Assert.NotEmpty(entities);
    }
}
