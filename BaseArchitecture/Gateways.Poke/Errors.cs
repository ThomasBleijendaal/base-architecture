using Common.Results;

namespace Gateways.Poke;

internal static class Errors
{
    private static readonly int System = 20000;

    public static ResultError GatewayException => new(System + 1, "Failed to connect with PokeAPI");
    public static ResultError TemporaryGatewayException => new(System + 2, "PokeAPI has temporary issue");
    public static ResultError UnknownType => new(System + 10, "Unknown type");
    public static ResultError GatewayError(int code, string message) => new(System + 1000 + code, $"PokeAPI error: {message}");
}
