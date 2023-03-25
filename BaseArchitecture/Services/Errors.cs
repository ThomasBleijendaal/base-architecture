namespace Services;

internal static class Errors
{
    private static readonly int System = 10000;

    public static ResultError FailedToGetPokémon => new(System + 1, "Failed to get Pokémon");
}
