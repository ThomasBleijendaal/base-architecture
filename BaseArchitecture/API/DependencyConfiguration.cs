using FluentValidation;

namespace API;

// TODO: add something that combines few calls + logic (like a query)
// TODO: add handler that reacts to failing gateway calls

public static class DependencyConfiguration
{
    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {
        services
            .AddServices()
            .AddSingleton<IObjectModelValidator, ValidatedObjectModelValidator>()
            .AddValidatorsFromAssemblyContaining<Program>();

        services
            .AddControllers(config =>
            {
                config.ModelBinderProviders.Insert(0, new ValidatedModelBinderProvider());

                config.Filters.Add<ValidatedContentFilter>();
            });

        return services;
    }
}
