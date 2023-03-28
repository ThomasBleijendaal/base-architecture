using Infrastructure.PokeDb.Entities;

namespace Infrastructure.PokeDb;

public interface IPokeRepository
{
    Task<int?> CreateAsync(Pokémon entity);
    Task<IReadOnlyList<Pokémon>> QueryAsync(); // TODO: query
    Task<Pokémon> GetByIdAsync(int id);
    Task UpdateAsync(Pokémon entity);
    Task DeleteAsync(int id);
    Task DeleteAsync(Pokémon entity);
}

public interface IEntity
{

}

public interface IRedisEntity<TEntity> : IEntity
{
    RedisKey GetRedisKey();

    RedisValue GetRedisValue();

    TEntity? GetEntity(RedisValue redisValue);
}

internal class PokeRepository : IPokeRepository
{
    private readonly IConnectionMultiplexer _connectionMultiplexer;

    public PokeRepository(
        IConnectionMultiplexer connectionMultiplexer)
    {
        _connectionMultiplexer = connectionMultiplexer;
    }

    public async Task<int?> CreateAsync(Pokémon entity)
    {
        var db = Map(entity);

        if (await GetDatabase().StringSetAsync(db.GetRedisKey(), db.GetRedisValue()))
        {
            return db.Id;
        }

        return null;
    }

    public Task DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(Pokémon entity)
    {
        throw new NotImplementedException();
    }

    public Task<Pokémon> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<IReadOnlyList<Pokémon>> QueryAsync()
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(Pokémon entity)
    {
        throw new NotImplementedException();
    }

    private IDatabase GetDatabase() => _connectionMultiplexer.GetDatabase();

    private static PokémonEntity Map(Pokémon domain)
        => new()
        {
            Id = domain.Id,
            Name = domain.Name
        };

    private static Pokémon Map(PokémonEntity entity)
        => new(entity.Id, entity.Name ?? "");
}
