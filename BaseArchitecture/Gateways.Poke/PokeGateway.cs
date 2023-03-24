using System.Net;
using Microsoft.Extensions.Logging;

namespace Gateways.Poke;

// TODO: the responses from gateway hide the reason why they fail which makes the query handler cannot
// see why stuff is failing

internal class PokeGateway : IPokeGateway
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<PokeGateway> _logger;

    public PokeGateway(
        HttpClient httpClient,
        ILogger<PokeGateway> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<Pokémon?> GetPokémonAsync(string name)
    {
        var request = new Request($"pokemon/{name}")
            .ExpectSuccessBody<PokémonResponse>(HttpStatusCode.NotFound)
            .ExpectErrorBody<ErrorResponse>(HttpStatusCode.InternalServerError);

        var result = await _httpClient.GetResultFromJsonAsync(request);

        if (!result.Success || result.SuccessValue == null)
        {
            _logger.LogError("{code}: {message}", result.ErrorValue?.Code ?? -1, result.ErrorValue?.Message ?? "some error");
            return null;
        }

        return new Pokémon(
            result.SuccessValue.Id,
            result.SuccessValue.Name,
            result.SuccessValue.Weight,
            result.SuccessValue.Height);
    }

    public async Task<IReadOnlyList<Pokémon>?> GetPokémonsAsync(int type)
    {
        var request = new Request<PokémonTypeCollectionResponse>($"type/{type}")
            .AllowNotFound();

        var result = await _httpClient.GetResultFromJsonAsync(request);

        if (!result.Success || result.Value == null)
        {
            return null;
        }

        return result.Value.Pokémons
            .Select(x => new Pokémon(
                x.Pokémon.Id,
                x.Pokémon.Name,
                x.Pokémon.Weight,
                x.Pokémon.Height))
            .ToArray();
    }
}
