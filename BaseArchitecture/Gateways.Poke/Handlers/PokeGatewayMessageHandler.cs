namespace Gateways.Poke.Handlers;

internal class PokeGatewayMessageHandler : DelegatingHandler
{
    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, CancellationToken cancellationToken)
    {
        request.Headers.Add("Accept", "application/json");
        return base.SendAsync(request, cancellationToken);
    }
}
