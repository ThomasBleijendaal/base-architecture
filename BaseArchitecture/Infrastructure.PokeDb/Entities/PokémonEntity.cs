using System.Text.Json;

namespace Infrastructure.PokeDb.Entities;

internal class PokémonEntity : IRedisEntity<PokémonEntity>
{
    public int Id { get; set; }
    public string? Name { get; set; }

    public RedisKey GetRedisKey()
    {
        var id = Id > 0 ? Id : Name?.GetHashCode() ?? GetHashCode();

        Id = id;

        return new RedisKey(id.ToString());
    }

    public RedisValue GetRedisValue()
    {
        return new RedisValue(JsonSerializer.Serialize(this));
    }

    public PokémonEntity? GetEntity(RedisValue redisValue)
    {
        return JsonSerializer.Deserialize<PokémonEntity>(redisValue.ToString());
    }
}
