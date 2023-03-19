namespace Gateways.Poke;

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
