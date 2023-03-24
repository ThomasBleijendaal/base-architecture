namespace Services;

// TODO: add something that combines few calls + logic (like a query)
// TODO: add handler that reacts to failing gateway calls

public static class DependencyConfiguration
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(typeof(DependencyConfiguration).Assembly);
            config.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidatedRequestBehavior<,>));
        });

        services.AddPokeGateway();

        services
            .AddValidatorsFromAssemblyContaining<GetPokémonsQuery>();

        return services;
    }
}
