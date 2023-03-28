using System.Diagnostics.CodeAnalysis;
using Infrastructure.PokeDb.Entities;

namespace Infrastructure.PokeDb;

// TODO: split and move parts to Common.Repository

public interface IPokeRepository
{
    Task<int?> CreateAsync(Pokémon item);
    Task<Pokémon?> GetByIdAsync(int id);
    Task<Pokémon?> GetByNameAsync(string name);
    Task UpdateAsync(Pokémon item);
    Task DeleteAsync(int id);
    Task DeleteAsync(Pokémon item);
}

public interface IEntity
{

}

public interface IRedisEntity<TEntity> : IEntity
{
    RedisValue GetRedisValue();

    IEnumerable<string> GetTags();

    abstract static TEntity? GetEntity(RedisValue redisValue);
}

internal class PokeRepository : IPokeRepository
{
    private readonly IConnectionMultiplexer _connectionMultiplexer;

    public PokeRepository(
        IConnectionMultiplexer connectionMultiplexer)
    {
        _connectionMultiplexer = connectionMultiplexer;
    }

    public async Task<int?> CreateAsync(Pokémon item)
    {
        var entity = Map(item);
        entity.Id = GenerateId(entity);

        if (await GetDatabase().StringSetAsync(GetKey(entity), entity.GetRedisValue()))
        {
            await UpdateTagsAsync(entity);
            return entity.Id;
        }

        return null;
    }

    public async Task DeleteAsync(int id)
    {
        await GetDatabase().StringGetDeleteAsync(GetKey(id));
    }

    public async Task DeleteAsync(Pokémon item)
    {
        await GetDatabase().StringGetDeleteAsync(GetKey(item.Id));
    }

    public async Task<Pokémon?> GetByIdAsync(int id)
        => await GetByRedisKeyAsync(GetKey(id));

    public async Task<Pokémon?> GetByNameAsync(string name)
    {
        var db = GetDatabase();

        var ids = await db.SetMembersAsync(GetTag(name));

        if (ids.Length != 1)
        {
            return null;
        }

        var key = new RedisKey(ids[0].ToString());

        return await GetByRedisKeyAsync(key);
    }

    private async Task<Pokémon?> GetByRedisKeyAsync(RedisKey key)
    {
        var redisValue = await GetDatabase().StringGetAsync(key);
        if (!redisValue.HasValue)
        {
            return null;
        }

        var entity = PokémonEntity.GetEntity(redisValue);

        return Map(entity);
    }

    public async Task UpdateAsync(Pokémon item)
    {
        var entity = Map(item);
        await GetDatabase().StringSetAsync(GetKey(entity), entity.GetRedisValue());
        await UpdateTagsAsync(entity);
    }

    private async Task UpdateTagsAsync(PokémonEntity entity)
    {
        var tags = entity.GetTags();
        var id = GetKey(entity);

        var db = GetDatabase();

        await Task.WhenAll(tags.Select(tag => db.SetAddAsync(GetTag(tag), id.ToString())));
    }

    private IDatabase GetDatabase() => _connectionMultiplexer.GetDatabase();

    private static PokémonEntity Map(Pokémon item)
        => new()
        {
            Id = item.Id,
            Name = item.Name,
            Height = item.Height,
            NrOfLikes = item.NrOfLikes,
            Weight = item.Weight
        };

    [return: NotNullIfNotNull(nameof(entity))]
    private static Pokémon? Map(PokémonEntity? entity)
        => entity == null ? null : new(entity.Id, entity.Name ?? "", entity.Weight, entity.Height, entity.NrOfLikes);

    private static RedisKey GetKey(PokémonEntity entity)
        => GetKey(entity.Id);

    private static RedisKey GetKey(int id)
        => new($"{nameof(Pokémon)}::{id}");
    private static RedisKey GetTag(string tag)
        => new($"{nameof(Pokémon)}:/tag/:{tag}");

    private static int GenerateId(PokémonEntity entity)
        => entity.GetHashCode();
}
