using System.Net;
using System.Text.Json;
using Polly;
using Polly.Contrib.Simmy;
using Polly.Contrib.Simmy.Latency;
using Polly.Contrib.Simmy.Outcomes;
using Polly.Extensions.Http;
using Polly.Timeout;

namespace Gateways.Poke;

public static class DependencyConfiguration
{
    public static IServiceCollection AddPokeGateway(this IServiceCollection services)
    {
        services.AddGatewayServices();

        services.AddTransient<PokeGatewayMessageHandler>();

        var retryPolicy = HttpPolicyExtensions
          .HandleTransientHttpError()
          .Or<TimeoutRejectedException>()
          .RetryAsync(0);

        var timeoutPolicy = Policy.TimeoutAsync<HttpResponseMessage>(1);

        var timeoutMonkeyPolicy = MonkeyPolicy.InjectLatencyAsync<HttpResponseMessage>(with => with
            .Latency(TimeSpan.FromSeconds(3))
            .InjectionRate(0.05)
            .Enabled(true));

        var notFoundMonkeyPolicy = MonkeyPolicy.InjectResultAsync<HttpResponseMessage>(with => with
            .Result((ctx, token) => Task.FromResult(new HttpResponseMessage(HttpStatusCode.NotFound)))
            .InjectionRate(0.05)
            .Enabled(true));

        var errorResponseMonkeyPolicy = MonkeyPolicy.InjectResultAsync<HttpResponseMessage>(with => with
            .Result((ctx, token) => Task.FromResult(new HttpResponseMessage(HttpStatusCode.InternalServerError)
            {
                Content = new StringContent(JsonSerializer.Serialize(new ErrorResponse(1, "Failure")))
            }))
            .InjectionRate(0.25)
            .Enabled(true));

        services.AddHttpClient<IPokeGateway, PokeGateway>(ConfigureClient)
            .AddPolicyHandler(retryPolicy)
            .AddPolicyHandler(timeoutPolicy)
            .AddHttpMessageHandler<PokeGatewayMessageHandler>()
            .AddHttpMessageHandler<LoggingHandler>()
            .AddPolicyHandler(timeoutMonkeyPolicy)
            .AddPolicyHandler(notFoundMonkeyPolicy)
            .AddPolicyHandler(errorResponseMonkeyPolicy);

        return services;
    }

    private static void ConfigureClient(HttpClient httpClient)
    {
        httpClient.BaseAddress = new Uri("https://pokeapi.co/api/v2/");
    }
}
