namespace Gateways.Poke.Models;

internal record PokémonCollectionResponse(
    int Id,
    [property: JsonPropertyName("results")] IReadOnlyList<PokémonResponse> Pokémons);
