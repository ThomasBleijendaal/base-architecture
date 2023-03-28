namespace Gateways.Poke;

public interface IPokeGateway
{
    Task<Response<PokémonDetailsResponse, ErrorResponse>> GetPokémonAsync(int name);
    Task<Response<PokémonDetailsResponse, ErrorResponse>> GetPokémonAsync(string name);
    Task<Response<PokémonCollectionResponse>> GetPokémonsAsync();
    Task<Response<PokémonTypeCollectionResponse>> GetPokémonsAsync(int type);
}
