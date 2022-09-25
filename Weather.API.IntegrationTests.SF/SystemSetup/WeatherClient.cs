using System.Net.Http.Json;
using Weather.API.IntegrationTests.SF.Models;

namespace Weather.API.IntegrationTests.SF.SystemSetup;

public class WeatherClient
{
    private HttpClient _client;

    public WeatherClient(HttpClient client)
    {
        _client = client;
    }

    public async Task<IEnumerable<WeatherForecast>?> GetAllWeatherUpdates()
    {
        return await _client.GetFromJsonAsync<IEnumerable<WeatherForecast>>("WeatherForecast/GetAll");
    }

    public async Task AddWeatherUpdateAsync(AddWeatherForecast request)
    {
        await _client.PostAsJsonAsync("WeatherForecast/Add", request);
    }
    
    public async Task<WeatherForecast> GetWeatherUpdate(GetWeatherForecast request)
    {
        var queryString = $"?Date={request.Date:s}&TemperatureC={request.TemperatureC}";
        return await _client.GetFromJsonAsync<WeatherForecast>("WeatherForecast/Get" + queryString);
    }
}