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
        /*
         * TODO:
         * 
         * V 1. 404 should be opt-in to accept
         * V 2. Extensions should be made to produce Result<T>
         * ? 3. Input validation
         * V 4. Polly + Chaos
         * 
         */

        var request = new Request<PokémonTypeCollection>($"type/{type}").AllowNotFound();

        var result = await _httpClient.GetResultFromJsonAsync(request);

        return result.Value?.Pokémons.Select(x => x.Pokémon).ToArray();
    }
}
