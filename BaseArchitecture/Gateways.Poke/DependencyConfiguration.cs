using Common;
using Gateways.Poke.Handlers;
using Microsoft.Extensions.DependencyInjection;

namespace Gateways.Poke;

public static class DependencyConfiguration
{
    public static IServiceCollection AddPokeGateway(this IServiceCollection services)
    {
        services.AddTransient<PokeGatewayMessageHandler>();

        services.AddHttpClient<IPokeGateway, PokeGateway>(httpClient =>
        {
            httpClient.BaseAddress = new Uri("https://pokeapi.co/api/v2/");
        })
            .AddHttpMessageHandler<PokeGatewayMessageHandler>()
            .AddHttpMessageHandler<LoggingHandler>();

        return services;
    }
}
