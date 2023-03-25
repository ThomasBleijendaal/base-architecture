using Common.Requests;
using MediatR;

namespace Common.Behaviors;

public class GenericExceptionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IValidatedRequest
    where TResponse : class
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        try
        {
            return await next.Invoke();
        }
        catch (Exception) when (typeof(Result)
            .GetMethod(nameof(Result.ExecutionError))
            ?.MakeGenericMethod(typeof(TResponse)
            .GetGenericArguments())
            ?.Invoke(null, new object?[] { new ResultError(-1, "Failure") })
            is TResponse response)
        {
            return response;
        }
    }
}
