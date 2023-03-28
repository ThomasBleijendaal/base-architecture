using System.Net;
using Common.Gateway.Responses;
using Polly.CircuitBreaker;
using Polly.Timeout;

namespace Services.Extensions;

internal static class ResponseExtensions
{
    public static Result GetGenericGatewayResult(this IResponse result)
    {
        if ((result.Exception is HttpRequestException http && http.StatusCode == HttpStatusCode.ServiceUnavailable) ||
            result.Exception is BrokenCircuitException ||
            result.Exception is TimeoutRejectedException)
        {
            return Result.TransientError(Errors.TemporaryGatewayException);
        }
        else
        {
            return Result.ExecutionError(Errors.GatewayException);
        }
    }

    public static Result<T> GetGenericGatewayResult<T>(this IResponse result)
    {
        if ((result.Exception is HttpRequestException http && http.StatusCode == HttpStatusCode.ServiceUnavailable) ||
            result.Exception is BrokenCircuitException ||
            result.Exception is TimeoutRejectedException)
        {
            return Result.TransientError<T>(Errors.TemporaryGatewayException);
        }
        else
        {
            return Result.ExecutionError<T>(Errors.GatewayException);
        }
    }
}
