﻿namespace Services;

public static class DependencyConfiguration
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(typeof(DependencyConfiguration).Assembly);
            config.AddBehavior(typeof(IPipelineBehavior<,>), typeof(GenericExceptionBehavior<,>));
            config.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidatedRequestBehavior<,>));
        });

        services.AddPokeGateway();

        services
            .AddValidatorsFromAssemblyContaining<GetPokémonsQuery>();

        return services;
    }
}
