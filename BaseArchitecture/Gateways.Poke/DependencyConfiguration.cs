using Polly;
using Polly.Contrib.Simmy;
using Polly.Contrib.Simmy.Latency;
using Polly.Extensions.Http;
using Polly.Timeout;

namespace Gateways.Poke;

public static class DependencyConfiguration
{
    public static IServiceCollection AddPokeGateway(this IServiceCollection services)
    {
        services.AddTransient<PokeGatewayMessageHandler>();

        var retryPolicy = HttpPolicyExtensions
          .HandleTransientHttpError()
          .Or<TimeoutRejectedException>()
          .RetryAsync(3);

        var timeoutPolicy = Policy.TimeoutAsync<HttpResponseMessage>(2);

        // TODO: add more monkeys 
        var timeoutMonkeyPolicy = MonkeyPolicy.InjectLatencyAsync<HttpResponseMessage>(with =>
        {
            with.Latency(TimeSpan.FromSeconds(3)).InjectionRate(0.5).Enabled(true);
        });

        services.AddHttpClient<IPokeGateway, PokeGateway>(ConfigureClient)
            .AddPolicyHandler(retryPolicy)
            .AddPolicyHandler(timeoutPolicy)
            .AddHttpMessageHandler<PokeGatewayMessageHandler>()
            .AddHttpMessageHandler<LoggingHandler>()
            .AddPolicyHandler(timeoutMonkeyPolicy);

        return services;
    }

    private static void ConfigureClient(HttpClient httpClient)
    {
        httpClient.BaseAddress = new Uri("https://pokeapi.co/api/v2/");
    }
}
