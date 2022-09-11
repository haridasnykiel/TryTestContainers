namespace Weather.API.Models;

public class AddForecastRequest
{
    public DateTime Date { get; set; }
    public int TemperatureC { get; set; }
    public string? Summary { get; set; }
}