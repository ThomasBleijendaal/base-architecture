using System.Text.Json;

namespace Infrastructure.PokeDb.Entities;

internal class PokémonEntity : IRedisEntity<PokémonEntity>
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public int Weight { get; set; }
    public int Height { get; set; }
    public int NrOfLikes { get; set; }

    public RedisValue GetRedisValue()
        => new(JsonSerializer.Serialize(this));

    public IEnumerable<string> GetTags()
        => string.IsNullOrWhiteSpace(Name) ? Array.Empty<string>() : new[] { Name };

    public static PokémonEntity? GetEntity(RedisValue redisValue)
        => JsonSerializer.Deserialize<PokémonEntity>(redisValue.ToString());
}
