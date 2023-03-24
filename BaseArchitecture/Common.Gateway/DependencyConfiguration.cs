namespace Common;

public static class DependencyConfiguration
{
    public static IServiceCollection AddGatewayServices(this IServiceCollection services)
    {
        services.AddTransient<LoggingHandler>();

        return services;
    }
}
