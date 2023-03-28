namespace Services.Queries;

public record SearchPokémonQuery(string? Name, int? Height, int? Weight) : ValidatedRequest<IReadOnlyList<Pokémon>>;
