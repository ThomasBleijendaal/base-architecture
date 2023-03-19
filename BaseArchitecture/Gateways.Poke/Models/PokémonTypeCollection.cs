namespace Gateways.Poke.Models;

public record PokémonTypeCollection(
    int Id,
    [property: JsonPropertyName("pokemon")] IReadOnlyList<PokémonTypeCollectionEntry> Pokémons);
