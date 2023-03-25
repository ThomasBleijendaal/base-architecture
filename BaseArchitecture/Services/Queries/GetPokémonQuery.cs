namespace Services.Queries;

public record GetPokémonQuery(string Name) : ValidatedRequest<PokémonDetails?>;
