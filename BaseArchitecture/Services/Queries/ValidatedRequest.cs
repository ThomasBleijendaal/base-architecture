namespace Services.Queries;

public record ValidatedRequest<T> : IRequest<Result<T>>, IValidatedRequest;
