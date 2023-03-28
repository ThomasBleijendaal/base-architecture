using Infrastructure.PokeDb;

namespace Services;

public static class DependencyConfiguration
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IPokeRepository, PokeRepository>();

        services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect("localhost"));

        return services;
    }
}
