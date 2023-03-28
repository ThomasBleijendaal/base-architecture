namespace Gateways.Poke.Models;

public record PokémonCollectionResponse(
    int Id,
    [property: JsonPropertyName("results")] IReadOnlyList<PokémonResponse> Pokémons);
