namespace Common.Gateway.Requests;

// TODO: this misses request bodies

public record Request(FormattableString Uri)
{
    public List<HttpStatusCode> SuccessCodes { get; private set; } = new();

    public Request AllowNotFound() { SuccessCodes.Add(HttpStatusCode.NotFound); return this; }

    public Request<TSuccess> ExpectSuccessBody<TSuccess>(params HttpStatusCode[] successCodes)
    {
        var request = new Request<TSuccess>(Uri);
        request.SuccessCodes.AddRange(successCodes);
        return request;
    }

    public string GetUrlSafeUri()
        => string.Format(
            Uri.Format,
            Uri.GetArguments()
                .Select(arg => (object?)WebUtility.UrlEncode(arg?.ToString()))
                .ToArray());
}

public record Request<TSuccess>(FormattableString Uri) : Request(Uri)
{
    public new Request<TSuccess> AllowNotFound() { SuccessCodes.Add(HttpStatusCode.NotFound); return this; }

    public Request<TSuccess, TError> ExpectErrorBody<TError>(params HttpStatusCode[] errorCodes)
    {
        var request = new Request<TSuccess, TError>(Uri);
        request.SuccessCodes.AddRange(SuccessCodes);
        request.ExpectErrorCodes(errorCodes);
        return request;
    }
}

public record Request<TSuccess, TError>(FormattableString Uri) : Request<TSuccess>(Uri)
{
    public new Request<TSuccess, TError> AllowNotFound() { SuccessCodes.Add(HttpStatusCode.NotFound); return this; }

    public List<HttpStatusCode> ErrorCodes { get; private set; } = new();

    public Request<TSuccess, TError> ExpectErrorCodes(params HttpStatusCode[] errorCodes)
    {
        ErrorCodes.AddRange(errorCodes);
        return this;
    }
}
