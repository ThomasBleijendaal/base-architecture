namespace Services.Handlers;

internal class SearchPokémonQueryHandler : IRequestHandler<SearchPokémonQuery, Result<IReadOnlyList<PokémonDetails>>>
{
    private readonly IPokeGateway _pokeGateway;

    public SearchPokémonQueryHandler(
        IPokeGateway pokeGateway)
    {
        _pokeGateway = pokeGateway;
    }

    public async Task<Result<IReadOnlyList<PokémonDetails>>> Handle(SearchPokémonQuery request, CancellationToken cancellationToken)
    {
        var result = await _pokeGateway.GetPokémonsAsync();

        if (!result.IsSuccess)
        {
            return Result.TransientError<IReadOnlyList<PokémonDetails>>(Errors.FailedToGetPokémon);
        }

        var filteredResults = result.Value
            ?.Where(x => x.Name.Contains(request.Name ?? "", StringComparison.InvariantCultureIgnoreCase))
            .ToArray()
            ?? Array.Empty<Pokémon>();

        var enrichedResults = (await Task.WhenAll(filteredResults
            .Select(async x =>
            {
                var result = await _pokeGateway.GetPokémonAsync(x.Name);

                if (!result.IsSuccess)
                {
                    return null;
                }

                return result.Value;
            })))
            .OfType<PokémonDetails>();

        if (request.Height.HasValue)
        {
            enrichedResults = enrichedResults.Where(x => x.Height == request.Height.Value);
        }

        if (request.Weight.HasValue)
        {
            enrichedResults = enrichedResults.Where(x => x.Weight == request.Weight.Value);
        }

        return Result.Success<IReadOnlyList<PokémonDetails>>(enrichedResults.ToArray());
    }
}
