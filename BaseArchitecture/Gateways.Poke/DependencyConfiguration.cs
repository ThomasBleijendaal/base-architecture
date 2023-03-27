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

        var breakerPolicy = Policy
            .HandleResult<HttpResponseMessage>(result => result.StatusCode == HttpStatusCode.InternalServerError)
            .CircuitBreakerAsync(2, TimeSpan.FromSeconds(60));

        var timeoutPolicy = Policy.TimeoutAsync<HttpResponseMessage>(1);

        var timeoutMonkeyPolicy = MonkeyPolicy.InjectLatencyAsync<HttpResponseMessage>(with => with
            .Latency(TimeSpan.FromSeconds(2))
            .InjectionRate(0.05)
            .Enabled(true));

        var notFoundMonkeyPolicy = MonkeyPolicy.InjectResultAsync<HttpResponseMessage>(with => with
            .Result((ctx, token) => Task.FromResult(new HttpResponseMessage(HttpStatusCode.NotFound)))
            .InjectionRate(0.05)
            .Enabled(true));

        var errorResponseMonkeyPolicy = MonkeyPolicy.InjectResultAsync<HttpResponseMessage>(with => with
            .Result((ctx, token) => Task.FromResult(new HttpResponseMessage(HttpStatusCode.InternalServerError)
            {
                Content = new StringContent(JsonSerializer.Serialize(new ErrorResponse(1, "Big failure")))
            }))
            .InjectionRate(0.25)
            .Enabled(true));

        var errorResponseMonkeyPolicy2 = MonkeyPolicy.InjectResultAsync<HttpResponseMessage>(with => with
            .Result((ctx, token) => Task.FromResult(new HttpResponseMessage(HttpStatusCode.ServiceUnavailable)
            {
                Content = new StringContent(JsonSerializer.Serialize(new ErrorResponse(2, "Some failure")))
            }))
            .InjectionRate(0.25)
            .Enabled(true));

        var monkeyPolicies = Policy.WrapAsync(
            timeoutMonkeyPolicy,
            notFoundMonkeyPolicy,
            errorResponseMonkeyPolicy,
            errorResponseMonkeyPolicy2);

        services.AddHttpClient<IPokeGateway, PokeGateway>(ConfigureClient)
            .AddHttpMessageHandler<LoggingHandler>()
            .AddPolicyHandler(retryPolicy)
            .AddPolicyHandler(timeoutPolicy)
            .AddPolicyHandler(breakerPolicy)
            .AddHttpMessageHandler<PokeGatewayMessageHandler>();
        // .AddPolicyHandler(monkeyPolicies);

        return services;
    }

    private static void ConfigureClient(HttpClient httpClient)
    {
        httpClient.BaseAddress = new Uri("https://pokeapi.co/api/v2/");
    }
}
