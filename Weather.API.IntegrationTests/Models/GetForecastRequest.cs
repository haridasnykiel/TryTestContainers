using System;

namespace Weather.API.IntegrationTests.Models;

public class GetWeatherForecast
{
    public DateTime Date { get; set; }
    public int TemperatureC { get; set; }
}