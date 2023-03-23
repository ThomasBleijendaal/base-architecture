﻿namespace Services.Handlers;

internal class GetPokémonsQueryHandler : IRequestHandler<GetPokémonsQuery, Result<IReadOnlyList<Pokémon>?>>
{
    private readonly IPokeGateway _pokeGateway;

    public GetPokémonsQueryHandler(
        IPokeGateway pokeGateway)
    {
        _pokeGateway = pokeGateway;
    }

    public async Task<Result<IReadOnlyList<Pokémon>?>> Handle(GetPokémonsQuery request, CancellationToken cancellationToken)
    {
        var list = await _pokeGateway.GetPokémonAsync(request.Level);

        if (list == null)
        {
            return Result.ExecutionError<IReadOnlyList<Pokémon>?>(Errors.FailedToGetPokémon);
        }

        return Result.Success(list)!;
    }
}
