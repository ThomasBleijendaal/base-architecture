namespace Services;

public static class DependencyConfiguration
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(typeof(GetPokémonQuery).Assembly);
            config.AddBehavior(typeof(IPipelineBehavior<,>), typeof(GenericExceptionBehavior<,>));
            config.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidatedRequestBehavior<,>));
        });

        services.AddPokeGateway();
        services.AddPokeDb();

        services
            .AddValidatorsFromAssemblyContaining<GetPokémonQuery>();

        return services;
    }
}
