using Polly.CircuitBreaker;
using Polly.Timeout;

namespace Gateways.Poke;

internal class PokeGateway : IPokeGateway
{
    private readonly HttpClient _httpClient;

    public PokeGateway(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<Result<PokémonDetails?>> GetPokémonAsync(string name)
    {
        var request = new Request($"pokemon/{name}")
            .ExpectSuccessBody<PokémonDetailsResponse>(HttpStatusCode.NotFound)
            .ExpectErrorBody<ErrorResponse>(HttpStatusCode.InternalServerError, HttpStatusCode.ServiceUnavailable);

        var result = await _httpClient.GetResultFromJsonAsync(request);

        if (!result.Success)
        {
            if (result.ErrorValue != null)
            {
                return Result.ExecutionError<PokémonDetails?>(Errors.GatewayError(result.ErrorValue.Code, result.ErrorValue.Message));
            }
            else
            {
                return GetGenericGatewayResult<PokémonDetails?>(result);
            }
        }

        return Result.Success(result.SuccessValue == null
            ? null
            : Map(result.SuccessValue));
    }

    public async Task<Result<IReadOnlyList<Pokémon>?>> GetPokémonsAsync()
    {
        var request = new Request<PokémonCollectionResponse>($"pokemon?limit=2000")
            .AllowNotFound();

        var result = await _httpClient.GetResultFromJsonAsync(request);

        if (!result.Success)
        {
            return GetGenericGatewayResult<IReadOnlyList<Pokémon>?>(result);
        }

        if (result.SuccessValue == null)
        {
            return Result.Success<IReadOnlyList<Pokémon>?>(Array.Empty<Pokémon>());
        }

        return Result.Success<IReadOnlyList<Pokémon>?>(result.SuccessValue.Pokémons
            .Select(Map)
            .ToArray())!;
    }

    public async Task<Result<IReadOnlyList<Pokémon>?>> GetPokémonsAsync(int type)
    {
        var request = new Request<PokémonTypeCollectionResponse>($"type/{type}")
            .AllowNotFound();

        var result = await _httpClient.GetResultFromJsonAsync(request);

        if (!result.Success)
        {
            return GetGenericGatewayResult<IReadOnlyList<Pokémon>?>(result);
        }

        if (result.SuccessValue == null)
        {
            return Result.ExecutionError<IReadOnlyList<Pokémon>?>(Errors.UnknownType);
        }

        return Result.Success<IReadOnlyList<Pokémon>?>(result.SuccessValue.Pokémons
            .Select(x => Map(x.Pokémon))
            .ToArray())!;
    }

    private static Result<T> GetGenericGatewayResult<T>(IResponse result)
    {
        if ((result.Exception is HttpRequestException http && http.StatusCode == HttpStatusCode.ServiceUnavailable) ||
            result.Exception is BrokenCircuitException ||
            result.Exception is TimeoutRejectedException)
        {
            return Result.TransientError<T>(Errors.TemporaryGatewayException);
        }
        else
        {
            return Result.ExecutionError<T>(Errors.GatewayException);
        }
    }

    private static PokémonDetails Map(PokémonDetailsResponse result)
        => new(
            result.Id,
            result.Name,
            result.Weight,
            result.Height);

    private static Pokémon Map(PokémonResponse result)
        => new(
            int.Parse(result.Url?.Split('/', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)[^1] ?? "0"),
            result.Name);
}
