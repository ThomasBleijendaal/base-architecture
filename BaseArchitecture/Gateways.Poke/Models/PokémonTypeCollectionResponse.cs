namespace Gateways.Poke.Models;

internal record PokémonTypeCollectionResponse(
    int Id,
    [property: JsonPropertyName("pokemon")] IReadOnlyList<PokémonTypeCollectionEntryResponse> Pokémons);
