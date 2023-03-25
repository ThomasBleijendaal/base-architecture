namespace Common;

public static class DependencyConfiguration
{
    public static IServiceCollection AddGatewayServices(this IServiceCollection services)
    {
        services.AddTransient<LoggingHandler>();

        // TODO: fix this

        //services.AddStackExchangeRedisCache(options =>
        //{
        //    options.Configuration = "localhost";
        //    options.InstanceName = "SampleInstance";
        //});

        return services;
    }
}
