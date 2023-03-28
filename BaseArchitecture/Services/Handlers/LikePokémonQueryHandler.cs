using Infrastructure.PokeDb;

namespace Services.Handlers;

internal class LikePokémonCommandHandler : IRequestHandler<LikePokémonCommand, Result>
{
    private readonly IPokeRepository _pokeRepository;
    private readonly IPokeGateway _pokeGateway;

    public LikePokémonCommandHandler(
        IPokeRepository pokeRepository,
        IPokeGateway pokeGateway)
    {
        _pokeRepository = pokeRepository;
        _pokeGateway = pokeGateway;
    }

    public async Task<Result> Handle(LikePokémonCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var entity = await _pokeRepository.GetByIdAsync(request.Id);

            if (entity == null)
            {
                var data = await _pokeGateway.GetPokémonAsync(request.Id);

                if (!data.Success || data.SuccessValue == null)
                {
                    return Result.TransientError(Errors.FailedToGetPokémon);
                }

                entity = new Pokémon(data.SuccessValue.Id, data.SuccessValue.Name, data.SuccessValue.Weight, data.SuccessValue.Height, 0);
            }

            entity = entity with { NrOfLikes = entity.NrOfLikes + 1 };

            await _pokeRepository.UpdateAsync(entity);

            return Result.Success();
        }
        catch
        {
            return Result.TransientError(Errors.FailedToLikePokémon);
        }
    }
}
