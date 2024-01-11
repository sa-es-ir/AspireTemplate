using Microsoft.Extensions.DependencyInjection;

namespace Redis.Cache;

public static class ServiceCollectionExtension
{
    public static void AddRedisCacheService(this IServiceCollection services)
    {
        services.AddScoped<RedisCacheService>();
    }
}
