namespace Services.Behaviors;

// TODO: move to common
internal class ValidatedRequestBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IValidatedRequest
    where TResponse : class
{
    private readonly IValidator<TRequest> _validator;

    public ValidatedRequestBehavior(
        IValidator<TRequest> validator)
    {
        _validator = validator;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (validationResult.IsValid)
        {
            return await next.Invoke();
        }

        var returnType = typeof(TResponse).GetGenericArguments();

        var method = typeof(Result).GetMethod(nameof(Result.ValidationError))?.MakeGenericMethod(returnType);

        TResponse? response;

        // TODO: map validation errors to a response model

        try
        {
            response = method?.Invoke(null, new object?[] { validationResult.Errors.ToArray() }) as TResponse;
        }
        catch
        {
            response = null;
        }

        return response ?? throw new ValidationException("Failed to construct Result", validationResult.Errors);
    }
}
