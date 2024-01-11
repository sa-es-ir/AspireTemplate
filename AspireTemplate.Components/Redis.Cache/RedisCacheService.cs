using StackExchange.Redis;
using System.Text.Json;

namespace Redis.Cache;

public class RedisCacheService(IConnectionMultiplexer redis)
{
    private readonly IDatabase _database = redis.GetDatabase();
    private static readonly JsonSerializerOptions jsonSerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true,
    };


    public async Task<bool> DeleteAsync(string key)
    {
        return await _database.KeyDeleteAsync(key);
    }

    public async Task<T?> GetAsync<T>(string key) where T : class
    {
        var data = await _database.StringGetAsync(key);

        if (data.IsNullOrEmpty)
        {
            return null;
        }

        return JsonSerializer.Deserialize<T>(data!, jsonSerializerOptions);
    }

    public async Task SaveAsync<T>(string key, T value) where T : class
    {
        await _database.StringSetAsync(key, JsonSerializer.Serialize(value, jsonSerializerOptions));
    }
}
