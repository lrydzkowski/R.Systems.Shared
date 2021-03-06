using R.Systems.Shared.FunctionalTests.Initializers;
using R.Systems.Shared.FunctionalTests.Services;
using R.Systems.Shared.WebApiTest;
using System.Net.Http;
using Xunit;

namespace R.Systems.Shared.FunctionalTests.Tests.EntityTests;

public class EntityControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    protected const string EntitiesUrl = "/entities";

    public EntityControllerTests(CustomWebApplicationFactory<Program> webApplicationFactory)
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
}
