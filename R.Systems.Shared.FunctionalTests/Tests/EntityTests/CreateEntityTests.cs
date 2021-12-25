using FluentAssertions;
using R.Systems.Shared.Core.Validation;
using R.Systems.Shared.WebApiTest.Models;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace R.Systems.Shared.FunctionalTests.Tests.EntityTests;

public class CreateEntityTests : EntityControllerTests
{
    [Fact]
    public async Task CreateEntity_WithoutAuthenticationToken_Unauthorized()
    {
        Entity entity = new()
        {
            Name = "Entity 1"
        };
        (HttpStatusCode httpStatusCode, _) = await RequestService.SendPostAsync<Entity, object>(
            EntitiesUrl,
            entity,
            HttpClient
        );

        Assert.Equal(HttpStatusCode.Unauthorized, httpStatusCode);
    }

    [Fact]
    public async Task CreateEntity_UserWithoutRoleAdmin_Forbidden()
    {
        string accessToken = AuthenticatorService.GenerateAccessToken(
            userId: 1,
            userEmail: "user@lukaszrydzkowski.pl",
            userRolesKeys: new List<string>() { "user" },
            privateKeyPem: RsaKeysService.GetPrivateKey()
        );
        Entity entity = new()
        {
            Name = "Entity 1"
        };

        (HttpStatusCode httpStatusCode, _) = await RequestService.SendPostAsync<Entity, object>(
            EntitiesUrl,
            entity,
            HttpClient,
            accessToken
        );

        Assert.Equal(HttpStatusCode.Forbidden, httpStatusCode);
    }

    [Fact]
    public async Task CreateEntity_IncorrectData_ReturnsErrorList()
    {
        string accessToken = AuthenticatorService.GenerateAccessToken(
            userId: 1,
            userEmail: "admin@lukaszrydzkowski.pl",
            userRolesKeys: new List<string>() { "admin" },
            privateKeyPem: RsaKeysService.GetPrivateKey()
        );
        Entity entity = new()
        {
            Name = ""
        };
        List<ErrorInfo> expectedErrors = new()
        {
            new ErrorInfo(errorKey: "IsRequired", elementKey: "EntityName")
        };

        (HttpStatusCode httpStatusCode, List<ErrorInfo>? errors) = await RequestService.SendPostAsync<Entity, List<ErrorInfo>>(
            EntitiesUrl,
            entity,
            HttpClient,
            accessToken
        );

        Assert.Equal(HttpStatusCode.BadRequest, httpStatusCode);
        errors.Should().BeEquivalentTo(expectedErrors);
    }

    [Fact]
    public async Task CreateEntity_CorrectData_ReturnsOk()
    {
        string accessToken = AuthenticatorService.GenerateAccessToken(
            userId: 1,
            userEmail: "admin@lukaszrydzkowski.pl",
            userRolesKeys: new List<string>() { "admin" },
            privateKeyPem: RsaKeysService.GetPrivateKey()
        );
        Entity entity = new()
        {
            Name = "Entity 1"
        };

        (HttpStatusCode httpStatusCode, _) = await RequestService.SendPostAsync<Entity, object>(
            EntitiesUrl,
            entity,
            HttpClient,
            accessToken
        );

        Assert.Equal(HttpStatusCode.OK, httpStatusCode);
    }
}
