namespace Common.Gateway.Handlers;

public class LoggingHandler : DelegatingHandler
{
    private readonly ILogger<LoggingHandler> _logger;

    public LoggingHandler(ILogger<LoggingHandler> logger)
    {
        _logger = logger;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Sending request to {method} {url} with headers {headers}", request.Method, request.RequestUri, string.Join(",", request.Headers.Select(x => x.Key)));

        var start = Stopwatch.GetTimestamp();

        var response = await base.SendAsync(request, cancellationToken);

        _logger.LogInformation("Request completed with {status} {reason} in {time} ms", (int)response.StatusCode, response.ReasonPhrase, Stopwatch.GetElapsedTime(start).TotalMilliseconds);

        return response;
    }
}
