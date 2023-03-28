namespace Services.Handlers;

internal class SearchPokémonQueryHandler : IRequestHandler<SearchPokémonQuery, Result<IReadOnlyList<Pokémon>>>
{
    private readonly IPokeGateway _pokeGateway;

    public SearchPokémonQueryHandler(
        IPokeGateway pokeGateway)
    {
        _pokeGateway = pokeGateway;
    }

    public async Task<Result<IReadOnlyList<Pokémon>>> Handle(SearchPokémonQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var response = await _pokeGateway.GetPokémonsAsync();

            if (!response.Success)
            {
                return response.GetGenericGatewayResult<IReadOnlyList<Pokémon>>();
            }

            if (response.SuccessValue == null)
            {
                return Result.Success<IReadOnlyList<Pokémon>>(Array.Empty<Pokémon>());
            }

            var filteredResults = response.SuccessValue.Pokémons
                ?.Where(x => x.Name.Contains(request.Name ?? "", StringComparison.InvariantCultureIgnoreCase))
                .ToArray()
                ?? Array.Empty<PokémonResponse>();

            var enrichedResults = (await Task.WhenAll(filteredResults
                .Select(async x =>
                {
                    var result = await _pokeGateway.GetPokémonAsync(x.Name);

                    if (!result.Success || result.SuccessValue == null)
                    {
                        return default;
                    }

                    return result.SuccessValue;
                })))
                .OfType<PokémonDetailsResponse>();

            if (request.Height.HasValue)
            {
                enrichedResults = enrichedResults.Where(x => x.Height == request.Height.Value);
            }

            if (request.Weight.HasValue)
            {
                enrichedResults = enrichedResults.Where(x => x.Weight == request.Weight.Value);
            }

            return Result.Success<IReadOnlyList<Pokémon>>(enrichedResults
                .Select(x => new Pokémon(x.Id, x.Name, x.Weight, x.Height, 0))
                .ToArray());
        }
        catch
        {
            return Result.ExecutionError<IReadOnlyList<Pokémon>>(Errors.FailedToGetPokémon);
        }
    }
}
