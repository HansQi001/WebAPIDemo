using Microsoft.AspNetCore.Mvc;
using WebAPIDemo.APIApp.Models;

namespace WebAPIDemo.APIApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeatherForecastController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<WeatherForecast>> GetWeatherForecast()
        {
            var summaries = new[] { "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching" };
            var forecast = Enumerable.Range(1, 5).Select(index =>
                                new WeatherForecast
                                (
                                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                                    Random.Shared.Next(-20, 55),
                                    summaries[Random.Shared.Next(summaries.Length)]
                                ))
                            .ToArray();
            return Ok(forecast);
        }
    }
}
