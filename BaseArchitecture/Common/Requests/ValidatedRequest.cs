using MediatR;

namespace Common.Requests;

public record ValidatedRequest : IRequest<Result>, IValidatedRequest;
public record ValidatedRequest<T> : IRequest<Result<T>>, IValidatedRequest;
