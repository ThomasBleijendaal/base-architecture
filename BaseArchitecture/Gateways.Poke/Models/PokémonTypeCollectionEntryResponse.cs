namespace Gateways.Poke.Models;

public record PokémonTypeCollectionEntryResponse(
    [property: JsonPropertyName("pokemon")] PokémonResponse Pokémon);
