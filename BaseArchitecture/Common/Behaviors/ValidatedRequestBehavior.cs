using Common.Requests;
using MediatR;

namespace Common.Behaviors;

public class ValidatedRequestBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IValidatedRequest
    where TResponse : class
{
    private readonly FluentValidation.IValidator<TRequest> _validator;

    public ValidatedRequestBehavior(
        FluentValidation.IValidator<TRequest> validator)
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

        var result = ValidationResult.Map(validationResult);

        var returnType = typeof(TResponse).GetGenericArguments();

        var method = typeof(Result).GetMethod(nameof(Result.ValidationError))?.MakeGenericMethod(returnType);

        TResponse? response;

        try
        {
            response = method?.Invoke(null, new object?[] { result }) as TResponse;
        }
        catch
        {
            response = null;
        }

        return response ?? throw new ValidationException(result.ValidationErrors ?? Enumerable.Empty<ValidationError>());
    }
}
