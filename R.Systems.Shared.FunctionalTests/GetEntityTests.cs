using R.Systems.Shared.FunctionalTests.Initializers;
using R.Systems.Shared.FunctionalTests.Services;
using R.Systems.Shared.WebApiTest.Models;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace R.Systems.Shared.FunctionalTests;

public class GetEntityTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private const string EntitiesUrl = "/entities";

    public GetEntityTests()
    {
        HttpClient = new CustomWebApplicationFactory<Program>().CreateClient();
        RequestService = new RequestService();
        AuthenticatorService = new AuthenticatorService();
        RsaKeysService = new RsaKeysService();
    }

    public HttpClient HttpClient { get; }
    internal RequestService RequestService { get; }
    internal AuthenticatorService AuthenticatorService { get; }
    internal RsaKeysService RsaKeysService { get; }

    [Fact]
    public async Task GetEntity_WithoutAuthenticationToken_Unauthorized()
    {
        long userId = 1;
        (HttpStatusCode httpStatusCode, Entity? entity) = await RequestService.SendGetAsync<Entity>(
            $"{EntitiesUrl}/{userId}",
            HttpClient
        );

        Assert.Equal(HttpStatusCode.Unauthorized, httpStatusCode);
        Assert.Null(entity);
    }

    [Fact]
    public async Task GetEntity_UserWithoutRoleAdmin_Forbidden()
    {
        long userId = 2;
        string accessToken = AuthenticatorService.GenerateAccessToken(
            userId: 1,
            userEmail: "user@lukaszrydzkowski.pl",
            userRolesKeys: new List<string>() { "user" },
            privateKeyPem: RsaKeysService.GetPrivateKey()
        );

        (HttpStatusCode httpStatusCode, Entity? entity) = await RequestService.SendGetAsync<Entity>(
            $"{EntitiesUrl}/{userId}",
            HttpClient,
            accessToken
        );

        Assert.Equal(HttpStatusCode.Forbidden, httpStatusCode);
        Assert.Null(entity);
    }

    [Fact]
    public async Task GetEntity_NotExistingId_NotFound()
    {
        long userId = 999;
        string accessToken = AuthenticatorService.GenerateAccessToken(
            userId: 1,
            userEmail: "admin@lukaszrydzkowski.pl",
            userRolesKeys: new List<string>() { "admin" },
            privateKeyPem: RsaKeysService.GetPrivateKey()
        );

        (HttpStatusCode httpStatusCode, Entity? entity) = await RequestService.SendGetAsync<Entity>(
            $"{EntitiesUrl}/{userId}",
            HttpClient,
            accessToken
        );

        Assert.Equal(HttpStatusCode.NotFound, httpStatusCode);
        Assert.Null(entity);
    }

    [Fact]
    public async Task GetEntity_CorrectEntityId_ReturnsEntityData()
    {
        long userId = 2;
        string accessToken = AuthenticatorService.GenerateAccessToken(
            userId: 1,
            userEmail: "admin@lukaszrydzkowski.pl",
            userRolesKeys: new List<string>() { "admin" },
            privateKeyPem: RsaKeysService.GetPrivateKey()
        );

        (HttpStatusCode httpStatusCode, Entity? entity) = await RequestService.SendGetAsync<Entity>(
            $"{EntitiesUrl}/{userId}",
            HttpClient,
            accessToken
        );

        Assert.Equal(HttpStatusCode.OK, httpStatusCode);
        Assert.Equal(userId, entity?.Id);
    }
}
