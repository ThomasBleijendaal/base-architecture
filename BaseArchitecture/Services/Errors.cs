namespace Services;

internal static class Errors
{
    public static ResultFailure FailedToGetPokémon => new ResultFailure(1, "Failed to get Pokémon");
}
