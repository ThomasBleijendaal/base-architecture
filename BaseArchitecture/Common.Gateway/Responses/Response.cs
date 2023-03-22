namespace Common.Gateway.Responses;

public record Response(bool Success);

public record Response<TSuccess>(bool Success, TSuccess? Value)
{

}

public record Response<TSuccess, TError>(bool Success, TSuccess? SuccessValue, TError? ErrorValue)
{

}
