namespace Gateways.Poke.Models;

public record PokémonTypeCollectionResponse(
    int Id,
    [property: JsonPropertyName("pokemon")] IReadOnlyList<PokémonTypeCollectionEntryResponse> Pokémons);
