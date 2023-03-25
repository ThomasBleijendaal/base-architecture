namespace Gateways.Poke;

public interface IPokeGateway
{
    Task<Result<PokémonDetails?>> GetPokémonAsync(string name);
    Task<Result<IReadOnlyList<Pokémon>?>> GetPokémonsAsync();
    Task<Result<IReadOnlyList<Pokémon>?>> GetPokémonsAsync(int type);
}
