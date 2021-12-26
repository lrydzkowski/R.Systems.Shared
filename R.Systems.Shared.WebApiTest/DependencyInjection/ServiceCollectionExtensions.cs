using R.Systems.Shared.Core.DependencyInjection;
using R.Systems.Shared.WebApi.DependencyInjection;

namespace R.Systems.Shared.WebApiTest.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static void AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAutomaticServices();
        services.AddJwtSettingsServices(configuration);
        services.AddJwtServices();
        services.AddSwaggerServices(swaggerPageTitle: "R.Systems.Shared.WebApiTest");
    }
}
