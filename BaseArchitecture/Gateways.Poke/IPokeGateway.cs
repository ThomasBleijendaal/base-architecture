namespace Gateways.Poke;

public interface IPokeGateway
{
    Task<Result<Pokémon?>> GetPokémonAsync(string name);
    Task<Result<IReadOnlyList<Pokémon>?>> GetPokémonsAsync();
    Task<Result<IReadOnlyList<Pokémon>?>> GetPokémonsAsync(int type);
}
