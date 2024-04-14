# Output Cache Using Redis Cache Store in .Net 8 C#

This repository demonstrates how to implement output caching in a .Net 8 C# application using Redis cache store.

## Prerequisites
- Redis Server
  Use the docker-compose file to spin up a new Redis server in docker.

## Add Redis Cache Store extension method as shown below
Implement methods of IOutputCacheStore interface
```
public class RedisOutputCacheStore : IOutputCacheStore
{   
    public async ValueTask<byte[]?> GetAsync(string key, CancellationToken cancellationToken)
    {       
    }
   public async ValueTask SetAsync(string key, byte[] value, string[]? tags, TimeSpan validFor, CancellationToken cancellationToken)
   {  
   }
   public async ValueTask EvictByTagAsync(string tag, CancellationToken cancellationToken)
   {
   }
}
```
Add an extension method to configure in services as below
```
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
```
## How to Use
Configure the Redis cache in the `Startup.cs` file:

```csharp
builder.Services.AddSingleton<IConnectionMultiplexer>(_=> ConnectionMultiplexer.Connect("localhost"));
builder.Services.AddRedisOutputCache();
```

Here, `localhost:6379` is the default connection string for the Redis server. Replace it with your Redis server's connection string.

In this code, we try to get the weather data from the cache. If it doesn't exist, we store the data in the cache and return it.


## Conclusion

This repository provides a basic example of how to use Redis as a output cache in a .NET 8 application. 
