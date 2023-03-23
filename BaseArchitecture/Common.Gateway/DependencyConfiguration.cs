namespace Common;

public static class DependencyConfiguration
{
    public static IServiceCollection AddGatewayServices(this IServiceCollection services)
    {
        services.AddSingleton<LoggingHandler>();

        return services;
    }
}
