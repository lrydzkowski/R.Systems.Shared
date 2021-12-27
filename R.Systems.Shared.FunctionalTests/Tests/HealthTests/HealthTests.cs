using R.Systems.Shared.FunctionalTests.Initializers;
using R.Systems.Shared.FunctionalTests.Services;
using R.Systems.Shared.WebApiTest;
using R.Systems.Shared.WebApiTest.Models.Responses;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace R.Systems.Shared.FunctionalTests.Tests.HealthTests
{
    public class HealthTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        protected const string EntitiesUrl = "/health";

        public HealthTests(CustomWebApplicationFactory<Program> webApplicationFactory)
        {
            HttpClient = webApplicationFactory.CreateClient();
            RequestService = new RequestService();
            AuthenticatorService = new AuthenticatorService();
            RsaKeysService = new RsaKeysService();
        }

        protected HttpClient HttpClient { get; }
        protected RequestService RequestService { get; }
        protected AuthenticatorService AuthenticatorService { get; }
        protected RsaKeysService RsaKeysService { get; }

        [Fact]
        public async Task GetLoggedUserId_CorrectData_ReturnsUserId()
        {
            long userId = 24;
            string accessToken = AuthenticatorService.GenerateAccessToken(
                userId,
                userEmail: "admin@lukaszrydzkowski.pl",
                userRolesKeys: new List<string>() { "admin" },
                privateKeyPem: RsaKeysService.GetPrivateKey()
            );

            (HttpStatusCode httpStatusCode, GetLoggedUserIdResponse? response)
                = await RequestService.SendGetAsync<GetLoggedUserIdResponse>(
                    $"{EntitiesUrl}/get-logged-user-id",
                    HttpClient,
                    accessToken
                );

            Assert.Equal(HttpStatusCode.OK, httpStatusCode);
            Assert.Equal(userId, response?.LoggedUserId);
        }

        [Fact]
        public async Task GetLoggedUserRoles_CorrectData_ReturnsUserRoles()
        {
            long userId = 24;
            List<string> userRolesKeys = new() { "admin" };
            string accessToken = AuthenticatorService.GenerateAccessToken(
                userId,
                userEmail: "admin@lukaszrydzkowski.pl",
                userRolesKeys,
                privateKeyPem: RsaKeysService.GetPrivateKey()
            );

            (HttpStatusCode httpStatusCode, GetLoggedUserRoles? response)
                = await RequestService.SendGetAsync<GetLoggedUserRoles>(
                    $"{EntitiesUrl}/get-roles",
                    HttpClient,
                    accessToken
                );

            Assert.Equal(HttpStatusCode.OK, httpStatusCode);
            Assert.Equal(userRolesKeys, response?.LoggedUserRoles);
        }
    }
}
