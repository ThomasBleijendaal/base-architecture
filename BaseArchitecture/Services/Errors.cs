namespace Services;

internal static class Errors
{
    public static ResultError FailedToGetPokémon => new(1, "Failed to get Pokémon");
}
