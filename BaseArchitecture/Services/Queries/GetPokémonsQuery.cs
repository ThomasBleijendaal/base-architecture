namespace Services.Queries;

public record GetPokémonsQuery(int Level) : ValidatedRequest<IReadOnlyList<Pokémon>>;

public record ValidatedRequest<T> : IRequest<Result<T>>, IValidatedRequest;

public interface IValidatedRequest { }
