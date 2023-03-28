using Infrastructure.PokeDb;

namespace Services.Handlers;

internal class GetPokémonQueryHandler : IRequestHandler<GetPokémonQuery, Result<Pokémon?>>
{
    private readonly IPokeRepository _pokeRepository;
    private readonly IPokeGateway _pokeGateway;

    public GetPokémonQueryHandler(
        IPokeRepository pokeRepository,
        IPokeGateway pokeGateway)
    {
        _pokeRepository = pokeRepository;
        _pokeGateway = pokeGateway;
    }

    public async Task<Result<Pokémon?>> Handle(GetPokémonQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var entity = await _pokeRepository.GetByNameAsync(request.Name);

            if (entity == null)
            {
                var response = await _pokeGateway.GetPokémonAsync(request.Name);

                if (!response.Success)
                {
                    if (response.ErrorValue != null)
                    {
                        // it might not be the best thing to return these gateway errors directly to the client
                        return Result.ExecutionError<Pokémon?>(Errors.GatewayError(response.ErrorValue.Code, response.ErrorValue.Message));
                    }
                    else
                    {
                        return response.GetGenericGatewayResult<Pokémon?>();
                    }
                }

                entity = response.SuccessValue == null
                    ? null
                    : new Pokémon(
                        response.SuccessValue.Id,
                        response.SuccessValue.Name,
                        response.SuccessValue.Weight,
                        response.SuccessValue.Height,
                        0);
            }

            return Result.Success(entity);
        }
        catch
        {
            return Result.ExecutionError<Pokémon?>(Errors.FailedToGetPokémon);
        }
    }
}
