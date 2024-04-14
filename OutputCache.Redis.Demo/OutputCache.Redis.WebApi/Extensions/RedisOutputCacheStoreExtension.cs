using Microsoft.AspNetCore.OutputCaching;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace OutputCache.Redis.WebApi.Extensions
{
    public static class RedisOutputCacheStoreExtension
    {
        public static IServiceCollection AddRedisOutputCache(this IServiceCollection services)
        {
            ArgumentNullException.ThrowIfNull(services);

            services.AddOutputCache();

            services.RemoveAll<IOutputCacheStore>();

            services.AddSingleton<IOutputCacheStore, RedisOutputCacheStore>();

            return services;
        }
    }
}
