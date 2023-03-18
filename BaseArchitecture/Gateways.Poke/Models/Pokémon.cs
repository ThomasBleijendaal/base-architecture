using System.Text.Json.Serialization;

namespace Gateways.Poke.Models;

public record Pokémon(string Name);

public record PokémonTypeCollectionEntry(
    [property: JsonPropertyName("pokemon")] Pokémon Pokémon);

public record PokémonTypeCollection(
    int Id,
    [property: JsonPropertyName("pokemon")] IReadOnlyList<PokémonTypeCollectionEntry> Pokémons);
