﻿using BuildingBlocks.Exceptions.Handler;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

namespace Ordering.API;

public static class DependencyInjection
{
    public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Add application services
        services.AddCarter();
        services.AddExceptionHandler<CustomExceptionHandler>();

        services.AddHealthChecks().AddSqlServer(configuration.GetConnectionString("Database")!);

        return services;
    }

    public static WebApplication UseApiServices(this WebApplication app)
    {
        // Use application services
        app.MapCarter();
        app.UseExceptionHandler(options => { });
        app.UseHealthChecks("/health", new HealthCheckOptions
        {
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });
        return app;
    }
}
