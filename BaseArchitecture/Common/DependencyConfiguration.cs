namespace Common;

public static class DependencyConfiguration
{
    public static IServiceCollection AddCommonServices(this IServiceCollection services)
    {
        services.AddSingleton<LoggingHandler>();

        return services;
    }
}
