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
        var request = new Request<PokémonTypeCollectionResponse>($"type/{type}").AllowNotFound();

        var result = await _httpClient.GetResultFromJsonAsync(request);

        if (!result.Success || result.Value == null)
        {
            return null;
        }

        return result.Value.Pokémons
            .Select(x => new Pokémon(x.Pokémon.Id, x.Pokémon.Name, x.Pokémon.Weight, x.Pokémon.Height))
            .ToArray();
    }
}
