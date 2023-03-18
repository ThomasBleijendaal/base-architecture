namespace Services.Queries;

public record GetPokémonsQuery(int Level) : IRequest<IReadOnlyList<Pokémon>>;
