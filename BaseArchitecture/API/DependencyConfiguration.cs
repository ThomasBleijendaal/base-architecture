using FluentValidation;

namespace API;

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
