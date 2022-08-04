using Microsoft.AspNetCore.Mvc;
using Weather.API.Repository;

namespace Weather.API.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly IWeatherRepository _repository;

    public WeatherForecastController(IWeatherRepository repository)
    {
        _repository = repository;
    }

    [HttpPost("Add")]
    public async Task AddWeather([FromBody] Forecast forecast)
    {
        await _repository.AddWeather(new WeatherForecast
        {
            Date = forecast.Date,
            TemperatureC = forecast.TemperatureC,
            Summary = forecast.Summary
        });
    }
    
    [HttpGet("GetAll")]
    public async Task<IEnumerable<WeatherForecast>> GetAll()
    {
        return await _repository.GetAll();
    }

    public class Forecast
    {
        public DateTime Date { get; set; }
        public int TemperatureC { get; set; }
        public string? Summary { get; set; }
    }
}
