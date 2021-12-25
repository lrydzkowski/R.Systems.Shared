using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using R.Systems.Shared.Core.Interfaces;
using R.Systems.Shared.FunctionalTests.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace R.Systems.Shared.FunctionalTests.Initializers;

public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        OverrideConfiguration(builder);
        builder.ConfigureServices(services =>
        {
            ReplaceIRsaKeysLoader(services);
        });
    }

    private void OverrideConfiguration(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((context, configBuilder) =>
        {
            configBuilder.AddInMemoryCollection(
                new Dictionary<string, string>
                {
                    ["Jwt:PublicKeyPemFilePath"] = "public.pem"
                }
            );
        });
    }

    private void ReplaceIRsaKeysLoader(IServiceCollection services)
    {
        RemoveService(services, typeof(IRsaKeys));
        services.AddSingleton<IRsaKeys, EmbeddedRsaKeys>();
    }

    private void RemoveService(IServiceCollection services, Type serviceType)
    {
        ServiceDescriptor? serviceDescriptor = services.FirstOrDefault(d => d.ServiceType == serviceType);
        if (serviceDescriptor != null)
        {
            services.Remove(serviceDescriptor);
        }
    }
}
