using System;

namespace Weather.API.IntegrationTests.Models;

public class AddWeatherForecast
{
    public DateTime Date { get; set; }

    public int TemperatureC { get; set; }

    public string? Summary { get; set; }
}