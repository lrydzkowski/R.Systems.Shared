using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using R.Systems.Shared.Core.Interfaces;
using R.Systems.Shared.Core.Models;
using R.Systems.Shared.WebApi.Jwt;
using R.Systems.Shared.WebApi.Swagger;

namespace R.Systems.Shared.WebApi.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static void AddJwtSettingsServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtVerificationSettings>(configuration.GetSection(JwtVerificationSettings.PropertyName));
        services.AddSingleton<IRsaKeys, RsaKeys>(ctx =>
        {
            JwtVerificationSettings? jwtSettings = ctx.GetRequiredService<IOptions<JwtVerificationSettings>>()?.Value;
            return new RsaKeys(jwtSettings?.PublicKeyPemFilePath, privateKeyPemFilePath: null);
        });
    }

    public static void AddJwtServices(this IServiceCollection services)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();
        services.ConfigureOptions<JwtBearerOptionsConfigurator>();
    }

    public static void AddSwaggerServices(this IServiceCollection services)
    {
        services.AddSwaggerGen();
        services.ConfigureOptions<SwaggerGenOptionsConfigurator>();
    }
}
