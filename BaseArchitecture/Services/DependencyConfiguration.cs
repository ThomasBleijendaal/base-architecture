using Microsoft.Extensions.DependencyInjection;
using Services.Handlers;

namespace Services;

public static class DependencyConfiguration
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(typeof(DependencyConfiguration).Assembly);
            config.AddBehavior(typeof(IPipelineBehavior<,>), typeof(Validation<,>));
        });

        services.AddPokeGateway();

        return services;
    }
}
