using FluentAssertions;
using R.Systems.Shared.Core.Validation;
using R.Systems.Shared.FunctionalTests.Initializers;
using R.Systems.Shared.WebApiTest;
using R.Systems.Shared.WebApiTest.Models;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace R.Systems.Shared.FunctionalTests.Tests.EntityTests;

public class UpdateEntityTests : EntityControllerTests
{
    public UpdateEntityTests(CustomWebApplicationFactory<Program> webApplicationFactory) : base(webApplicationFactory)
    {
    }

    [Fact]
    public async Task UpdateEntity_WithoutAuthenticationToken_Unauthorized()
    {
        Entity entity = new()
        {
            Id = 1,
            Name = "Entity 1"
        };
        (HttpStatusCode httpStatusCode, _) = await RequestService.SendPostAsync<Entity, object>(
            $"{EntitiesUrl}/{entity.Id}",
            entity,
            HttpClient
        );

        Assert.Equal(HttpStatusCode.Unauthorized, httpStatusCode);
    }

    [Fact]
    public async Task UpdateEntity_UserWithoutRoleAdmin_Forbidden()
    {
        string accessToken = AuthenticatorService.GenerateAccessToken(
            userId: 1,
            userEmail: "user@lukaszrydzkowski.pl",
            userRolesKeys: new List<string>() { "user" },
            privateKeyPem: RsaKeysService.GetPrivateKey()
        );
        Entity entity = new()
        {
            Id = 1,
            Name = "Entity 1"
        };

        (HttpStatusCode httpStatusCode, _) = await RequestService.SendPostAsync<Entity, object>(
            $"{EntitiesUrl}/{entity.Id}",
            entity,
            HttpClient,
            accessToken
        );

        Assert.Equal(HttpStatusCode.Forbidden, httpStatusCode);
    }

    [Fact]
    public async Task UpdateEntity_IncorrectData_ReturnsErrorList()
    {
        string accessToken = AuthenticatorService.GenerateAccessToken(
            userId: 1,
            userEmail: "admin@lukaszrydzkowski.pl",
            userRolesKeys: new List<string>() { "admin" },
            privateKeyPem: RsaKeysService.GetPrivateKey()
        );
        Entity entity = new()
        {
            Id = 999,
            Name = ""
        };
        List<ErrorInfo> expectedErrors = new()
        {
            new ErrorInfo(errorKey: "NotExist", elementKey: "EntityId"),
            new ErrorInfo(errorKey: "IsRequired", elementKey: "EntityName")
        };

        (HttpStatusCode httpStatusCode, List<ErrorInfo>? errors) = await RequestService.SendPostAsync<Entity, List<ErrorInfo>>(
            $"{EntitiesUrl}/{entity.Id}",
            entity,
            HttpClient,
            accessToken
        );

        Assert.Equal(HttpStatusCode.BadRequest, httpStatusCode);
        errors.Should().BeEquivalentTo(expectedErrors);
    }

    [Fact]
    public async Task UpdateEntity_CorrectData_ReturnsOk()
    {
        string accessToken = AuthenticatorService.GenerateAccessToken(
            userId: 1,
            userEmail: "admin@lukaszrydzkowski.pl",
            userRolesKeys: new List<string>() { "admin" },
            privateKeyPem: RsaKeysService.GetPrivateKey()
        );
        Entity entity = new()
        {
            Id = 1,
            Name = "Entity 1"
        };

        (HttpStatusCode httpStatusCode, _) = await RequestService.SendPostAsync<Entity, object>(
            $"{EntitiesUrl}/{entity.Id}",
            entity,
            HttpClient,
            accessToken
        );

        Assert.Equal(HttpStatusCode.OK, httpStatusCode);
    }
}
