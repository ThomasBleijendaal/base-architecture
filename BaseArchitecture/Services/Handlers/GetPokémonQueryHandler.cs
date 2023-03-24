namespace Services.Handlers;

internal class GetPokémonQueryHandler : IRequestHandler<GetPokémonQuery, Result<Pokémon?>>
{
    private readonly IPokeGateway _pokeGateway;

    public GetPokémonQueryHandler(
        IPokeGateway pokeGateway)
    {
        _pokeGateway = pokeGateway;
    }

    public async Task<Result<Pokémon?>> Handle(GetPokémonQuery request, CancellationToken cancellationToken)
    {
        var response = await _pokeGateway.GetPokémonAsync(request.Name);

        if (response == null)
        {
            return Result.TransientError<Pokémon?>(Errors.FailedToGetPokémon);
        }

        return Result.Success(response)!;
    }
}
