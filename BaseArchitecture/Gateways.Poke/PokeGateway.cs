using System.Text.Json;

namespace Gateways.Poke;

internal class PokeGateway : IPokeGateway
{
    private readonly HttpClient _httpClient;
    private readonly static JsonSerializerOptions JsonSerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        AllowTrailingCommas = true
    };

    public PokeGateway(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public Task<Response<PokémonDetailsResponse, ErrorResponse>> GetPokémonAsync(int id) => GetPokémonByNameOrIdAsync(id.ToString());

    public Task<Response<PokémonDetailsResponse, ErrorResponse>> GetPokémonAsync(string name) => GetPokémonByNameOrIdAsync(name);

    public async Task<Response<PokémonCollectionResponse>> GetPokémonsAsync()
    {
        var request = new Request<PokémonCollectionResponse>($"pokemon?limit=2000")
            .AllowNotFound();

        return await _httpClient.SendAsync(request, JsonSerializerOptions);
    }

    public async Task<Response<PokémonTypeCollectionResponse>> GetPokémonsAsync(int type)
    {
        var request = new Request<PokémonTypeCollectionResponse>($"type/{type}")
            .AllowNotFound();

        return await _httpClient.SendAsync(request, JsonSerializerOptions);
    }

    private async Task<Response<PokémonDetailsResponse, ErrorResponse>> GetPokémonByNameOrIdAsync(string idOrName)
    {
        var request = new Request($"pokemon/{idOrName}")
            .ExpectSuccessBody<PokémonDetailsResponse>(HttpStatusCode.NotFound)
            .ExpectErrorBody<ErrorResponse>(HttpStatusCode.InternalServerError, HttpStatusCode.ServiceUnavailable);

        return await _httpClient.SendAsync(request, JsonSerializerOptions);
    }
}
