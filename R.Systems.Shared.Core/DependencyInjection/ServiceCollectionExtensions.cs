﻿using Microsoft.Extensions.DependencyInjection;
using R.Systems.Shared.Core.Interfaces;

namespace R.Systems.Shared.Core.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static void AddAutomaticServices(this IServiceCollection services)
    {
        AddScopedServices(services);
        AddTransientServices(services);
        AddSingletonServices(services);
    }

    private static void AddScopedServices(IServiceCollection services)
    {
        services.Scan(scan => scan.FromApplicationDependencies()
                .AddClasses(classes => classes.AssignableTo<IDependencyInjectionScoped>())
                .AsSelf()
                .WithScopedLifetime()
            );
    }

    private static void AddTransientServices(IServiceCollection services)
    {
        services.Scan(scan => scan.FromApplicationDependencies()
                .AddClasses(classes => classes.AssignableTo<IDependencyInjectionTransient>())
                .AsSelf()
                .WithTransientLifetime()
            );
    }

    private static void AddSingletonServices(IServiceCollection services)
    {
        services.Scan(scan => scan.FromApplicationDependencies()
                .AddClasses(classes => classes.AssignableTo<IDependencyInjectionSingleton>())
                .AsSelf()
                .WithSingletonLifetime()
            );
    }
}
