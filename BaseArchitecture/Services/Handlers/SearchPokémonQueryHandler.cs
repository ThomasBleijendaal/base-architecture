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
        var result = await _pokeGateway.GetPokémonsAsync();

        if (!result.IsSuccess)
        {
            return Result.TransientError<IReadOnlyList<Pokémon>>(Errors.FailedToGetPokémon);
        }

        // TODO: filter on height and weight

        var filteredResults = result.Value
            ?.Where(x => x.Name.Contains(request.Name ?? "", StringComparison.InvariantCultureIgnoreCase))
            .ToArray()
            ?? Array.Empty<Pokémon>();

        return Result.Success<IReadOnlyList<Pokémon>>(filteredResults);
    }
}
