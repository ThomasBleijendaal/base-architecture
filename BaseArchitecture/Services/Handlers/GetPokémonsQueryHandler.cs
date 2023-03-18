namespace Services.Handlers;

internal class GetPokémonsQueryHandler : IRequestHandler<GetPokémonsQuery, IReadOnlyList<Pokémon>>
{
    private readonly IPokeGateway _pokeGateway;

    public GetPokémonsQueryHandler(
        IPokeGateway pokeGateway)
    {
        _pokeGateway = pokeGateway;
    }

    public async Task<IReadOnlyList<Pokémon>> Handle(GetPokémonsQuery request, CancellationToken cancellationToken)
    {
        var list = await _pokeGateway.GetPokémonAsync(request.Level);

        return list ?? new List<Pokémon>();
    }
}

internal class Validation<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
     where TRequest : notnull
{
    public Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        return next.Invoke();
    }
}
