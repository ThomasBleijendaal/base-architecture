using Common;
using FluentValidation;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace API;

public static class DependencyConfiguration
{
    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {
        services
            .AddServices()
            .AddValidatorsFromAssemblyContaining<Program>();

        services.AddOpenTelemetry()
            .WithTracing(tracerProviderBuilder => tracerProviderBuilder
                .AddSource(DiagnosticsConfig.ActivitySource.Name)
                .ConfigureResource(resource => resource.AddService(DiagnosticsConfig.ServiceName))
                .AddAspNetCoreInstrumentation()
                .AddJaegerExporter())
            .WithMetrics(metricsProviderBuilder => metricsProviderBuilder
                .ConfigureResource(resource => resource.AddService(DiagnosticsConfig.ServiceName))
                .AddAspNetCoreInstrumentation());

        services
            .AddControllers(config =>
            {
                config.ModelBinderProviders.Insert(0, new ValidatedModelBinderProvider());

                config.Filters.Add<ValidatedContentFilter>();
            });

        return services;
    }
}
