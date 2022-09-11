using Microsoft.AspNetCore.Mvc;
using Weather.API.Models;
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
    public async Task<IActionResult> AddWeather([FromBody] AddForecastRequest addForecast)
    {

        if (addForecast.Date == DateTime.MinValue)
        {
            return BadRequest("Date must be set");
        }

        if (string.IsNullOrEmpty(addForecast.Summary))
        {
            return BadRequest("A summary must be added");
        }
        
        var numberOfForecastsAdded = await _repository.AddWeather(new WeatherForecast
        {
            Date = addForecast.Date,
            TemperatureC = addForecast.TemperatureC,
            Summary = addForecast.Summary
        });

        return Ok($"Number of forecasts added {numberOfForecastsAdded}");
    }
    
    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAll()
    {
        var results = await _repository.GetAll();

        return Ok(results);
    }
    
    [HttpGet("Get")]
    public async Task<IActionResult> Get([FromQuery] GetForecastRequest request)
    {
        if (request.Date == DateTime.MinValue)
        {
            return BadRequest("Must include date");
        }
        
        var result = await _repository.Get(request.Date, request.TemperatureC);

        return Ok(result);
    }
}
