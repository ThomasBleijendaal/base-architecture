namespace Gateways.Poke.Models;

public record Pokémon(
    int Id,
    string Name,
    int Weight,
    int Height,
    int NrOfLikes);
