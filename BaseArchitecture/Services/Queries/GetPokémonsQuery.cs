namespace Services.Queries;

public record GetPokémonsQuery(int Type) : ValidatedRequest<IReadOnlyList<Pokémon>?>;
