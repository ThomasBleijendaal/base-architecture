using System.Net.Http.Json;
using Common;
using Gateways.Poke.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Gateways.Poke;

public interface IPokeGateway
{
    Task<IReadOnlyList<Pokémon>?> GetPokémonAsync(int type);
}

internal class PokeGateway : IPokeGateway
{
    private readonly HttpClient _httpClient;

    public PokeGateway(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IReadOnlyList<Pokémon>?> GetPokémonAsync(int type)
    {
        // TODO: how to handle failures here?

        var result = await _httpClient.GetFromJsonAsync<PokémonTypeCollection>($"type/{type}");

        return result?.Pokémons.Select(x => x.Pokémon).ToArray();
    }
}

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

internal class PokeGatewayMessageHandler : DelegatingHandler
{
    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, CancellationToken cancellationToken)
    {
        request.Headers.Add("Accept", "application/json");
        return base.SendAsync(request, cancellationToken);
    }
}
