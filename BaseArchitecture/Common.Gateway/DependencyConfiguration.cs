using OpenTelemetry.Trace;

namespace Common;

public static class DependencyConfiguration
{
    public static IServiceCollection AddGatewayServices(this IServiceCollection services)
    {
        services.AddTransient<LoggingHandler>();

        // TODO: move this to Common.Caching + some helper with that does stale cache handling

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = "localhost";
            options.InstanceName = "SampleInstance";
        });

        services.AddOpenTelemetry()
            .WithTracing(traceProviderBuilder => traceProviderBuilder
                .AddHttpClientInstrumentation());

        return services;
    }
}
