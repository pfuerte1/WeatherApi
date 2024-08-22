using Microsoft.AspNetCore.Mvc;
using WeatherApi.Services;

namespace WeatherApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly WeatherForecastService _weatherForecastService;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, WeatherForecastService weatherForecastService)
        {
            _logger = logger;
            _weatherForecastService = weatherForecastService;
        }

        [HttpGet("coordinates/{latitude},{longitude}")]
        public async Task<IActionResult> GetWeatherForecastByCoodinatesAsync(double latitude, double longitude)
        {
            var result = await _weatherForecastService.GetForecastByCoordinatesAsync(latitude, longitude);
            return Ok(result);
        }
    }
}
