using Microsoft.AspNetCore.Mvc;
using SourceGeneratorTest.Data.Model;

namespace SourceGeneratorTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = { "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching" };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }


        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<WeatherForecast>), 200)]
        public IActionResult GetWeatherForecasts()
        {
            return Ok(Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray());
        }

        [HttpGet("/{id}")]
        [ProducesResponseType(typeof(WeatherForecast), 200)]
        public IActionResult GetWeatherForecastById(int id)
        {
            return Ok(new WeatherForecast
            {
                Date = DateTime.Now.AddDays(id),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            });
        }

        [HttpPost("/")]
        [ProducesResponseType(typeof(void), 201)]
        public IActionResult PostWeatherForecast([FromBody] WeatherForecast request)
        {
            //do something
            return Ok();
        }
    }
}