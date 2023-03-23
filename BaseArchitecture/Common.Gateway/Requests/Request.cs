namespace Common.Gateway.Requests;

// TODO: this misses request bodies

public record Request(string Uri)
{
    public List<HttpStatusCode> SuccessCodes { get; private set; } = new();

    public Request AllowNotFound() { SuccessCodes.Add(HttpStatusCode.NotFound); return this; }

    public Request<TSuccess> ExpectSuccessBody<TSuccess>(params HttpStatusCode[] successCodes)
    {
        var request = new Request<TSuccess>(Uri);
        request.SuccessCodes.AddRange(successCodes);
        return request;
    }
}

public record Request<TSuccess>(string Uri) : Request(Uri)
{
    public new Request<TSuccess> AllowNotFound() { SuccessCodes.Add(HttpStatusCode.NotFound); return this; }

    public Request<TSuccess, TError> ExpectErrorBody<TError>(params HttpStatusCode[] errorCodes)
    {
        var request = new Request<TSuccess, TError>(Uri);
        request.SuccessCodes.AddRange(errorCodes);
        return request;
    }
}

public record Request<TSuccess, TError>(string Uri) : Request<TSuccess>(Uri)
{
    public new Request<TSuccess, TError> AllowNotFound() { SuccessCodes.Add(HttpStatusCode.NotFound); return this; }

    public List<HttpStatusCode> ErrorCodes { get; private set; } = new();

    public Request<TSuccess, TError> ExpectErrorCodes(params HttpStatusCode[] errorCodes)
    {
        ErrorCodes.AddRange(errorCodes);
        return this;
    }
}
