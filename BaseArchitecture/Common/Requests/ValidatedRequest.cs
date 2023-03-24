using MediatR;

namespace Common.Requests;

public record ValidatedRequest<T> : IRequest<Result<T>>, IValidatedRequest;
