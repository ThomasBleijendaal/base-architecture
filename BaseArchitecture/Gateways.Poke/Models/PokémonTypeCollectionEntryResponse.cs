namespace Gateways.Poke.Models;

internal record PokémonTypeCollectionEntryResponse(
    [property: JsonPropertyName("pokemon")] PokémonResponse Pokémon);
