namespace Common.Gateway.Requests;

public record Request : IRequest
{
    public Request(FormattableString uri)
    {
        Uri = uri;
    }

    public Request(HttpMethod httpMethod, FormattableString uri)
    {
        HttpMethod = httpMethod;
        Uri = uri;
    }

    public HttpMethod HttpMethod { get; } = HttpMethod.Get;

    public FormattableString Uri { get; private set; }

    public BinaryData? Content { get; set; }

    public List<HttpStatusCode> SuccessCodes { get; private set; } = new();

    public Request AllowNotFound() { SuccessCodes.Add(HttpStatusCode.NotFound); return this; }

    public Request<TSuccess> ExpectSuccessBody<TSuccess>(params HttpStatusCode[] successCodes)
    {
        var request = new Request<TSuccess>(HttpMethod, Uri);
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

public record Request<TSuccess> : Request
{
    public Request(FormattableString uri) : base(uri)
    {
    }

    public Request(HttpMethod httpMethod, FormattableString uri) : base(httpMethod, uri)
    {
    }

    public new Request<TSuccess> AllowNotFound() { base.AllowNotFound(); return this; }

    public Request<TSuccess, TError> ExpectErrorBody<TError>(params HttpStatusCode[] errorCodes)
    {
        var request = new Request<TSuccess, TError>(HttpMethod, Uri);
        request.SuccessCodes.AddRange(SuccessCodes);
        request.ExpectErrorCodes(errorCodes);
        return request;
    }
}

public record Request<TSuccess, TError> : Request<TSuccess>
{
    public Request(FormattableString uri) : base(uri)
    {
    }

    public Request(HttpMethod httpMethod, FormattableString uri) : base(httpMethod, uri)
    {
    }

    public new Request<TSuccess, TError> AllowNotFound() { base.AllowNotFound(); return this; }

    public List<HttpStatusCode> ErrorCodes { get; private set; } = new();

    public Request<TSuccess, TError> ExpectErrorCodes(params HttpStatusCode[] errorCodes)
    {
        ErrorCodes.AddRange(errorCodes);
        return this;
    }
}
