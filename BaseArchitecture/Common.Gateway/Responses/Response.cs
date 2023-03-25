namespace Common.Gateway.Responses;

public record Response(bool Success) : IResponse
{
    public Exception? Exception { get; internal set; }
};

public record Response<TSuccess>(bool Success, TSuccess? SuccessValue) : Response(Success)
{
}

public record Response<TSuccess, TError>(bool Success, TSuccess? SuccessValue, TError? ErrorValue) : Response<TSuccess>(Success, SuccessValue)
{
}
