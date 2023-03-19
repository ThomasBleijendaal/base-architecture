namespace Services.Behaviors;

internal class ValidatedRequestBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IValidatedRequest
{
    public Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (request is ValidatedRequest<IReadOnlyList<Pokémon>> validatedResponse)
        {

        }

        return next.Invoke();
    }
}
