using Gateways.Poke.Models;

namespace Gateways.Poke;

public interface IPokeGateway
{
    Task<IReadOnlyList<Pokémon>?> GetPokémonAsync(int type);
}
