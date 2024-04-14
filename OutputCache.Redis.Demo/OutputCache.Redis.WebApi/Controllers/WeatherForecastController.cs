using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace OutputCache.Redis.WebApi.Controllers
{
    [ApiController]
    [Route("/api/[controller]/[action]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
       
        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;          
        }

        [HttpGet]
        [OutputCache(Duration = 300, Tags = ["WeatherData"])]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpPost]
        public async Task InvalidateCache([FromServices] IOutputCacheStore outputCacheStore)
        {
            await outputCacheStore.EvictByTagAsync("WeatherData",new CancellationToken());
        }
    }
}
