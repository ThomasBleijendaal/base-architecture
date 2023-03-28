namespace Services;

internal static class Errors
{
    private static readonly int System = 10000;

    public static ResultError FailedToGetPokémon => new(System + 1, "Failed to get Pokémon");
    public static ResultError GatewayException => new(System + 2, "Failed to connect with PokeAPI");
    public static ResultError TemporaryGatewayException => new(System + 3, "PokeAPI has temporary issue");
    public static ResultError FailedToLikePokémon => new(System + 4, "Failed to get Pokémon");
    public static ResultError GatewayError(int code, string message) => new(System + 1000 + code, $"PokeAPI error: {message}");
}
