using Microsoft.AspNetCore.OutputCaching;
using StackExchange.Redis;

namespace OutputCache.Redis.WebApi.Extensions
{
    public class RedisOutputCacheStore : IOutputCacheStore
    {
        private readonly IConnectionMultiplexer connectionMultiplexer;

        public RedisOutputCacheStore(IConnectionMultiplexer connectionMultiplexer)
        {
            this.connectionMultiplexer = connectionMultiplexer;
        }

        public async ValueTask<byte[]?> GetAsync(string key, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(key, nameof(key));
            var db = connectionMultiplexer.GetDatabase();
            return await db.StringGetAsync(key);
        }

        public async ValueTask SetAsync(string key, byte[] value, string[]? tags, TimeSpan validFor, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(key,nameof(key));
            ArgumentNullException.ThrowIfNull(value, nameof(value));

            var db = connectionMultiplexer.GetDatabase();

            foreach(string tag in tags?? Array.Empty<string>())
            {
               await db.SetAddAsync(tag, key);
            }
            await db.StringSetAsync(key, value);
        }

        public async ValueTask EvictByTagAsync(string tag, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(tag, nameof(tag));
          
            var db = connectionMultiplexer.GetDatabase();

            var cachedKeys = await db.SetMembersAsync(tag);

            var keys = cachedKeys
                       .Select(x => (RedisKey)x.ToString())
                       .Concat(new[] { (RedisKey)tag })
                       .ToArray();

            await db.KeyDeleteAsync(keys);

        }
    }
}
