namespace Weather.API.Models;

public class GetForecastRequest
{
    public DateTime Date { get; set; }
    public int TemperatureC { get; set; }
}