namespace Services.Queries;

public record GetPokémonsQuery(int Level) : ValidatedRequest<IReadOnlyList<Pokémon>>;
