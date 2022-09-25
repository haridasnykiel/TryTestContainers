namespace Weather.API.IntegrationTests.SF.Models;

public class AddWeatherForecast
{
    public DateTime Date { get; set; }

    public int TemperatureC { get; set; }

    public string? Summary { get; set; }
}