namespace Gateways.Poke;

public interface IPokeGateway
{
    Task<Pokémon?> GetPokémonAsync(string name);
    Task<IReadOnlyList<Pokémon>?> GetPokémonsAsync(int type);
}
