namespace Gateways.Poke.Models;

public record PokémonTypeCollectionEntry(
    [property: JsonPropertyName("pokemon")] Pokémon Pokémon);
