using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Weather.API.IntegrationTests.Models;

namespace Weather.API.IntegrationTests;

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

    public async Task AddWeatherUpdate(AddWeatherForecast request)
    {
        await _client.PostAsJsonAsync("WeatherForecast/Add", request);
    }
    
    public async Task<WeatherForecast> GetWeatherUpdate(GetWeatherForecast request)
    {
        var queryString = $"?Date={request.Date:s}&TemperatureC={request.TemperatureC}";
        return await _client.GetFromJsonAsync<WeatherForecast>("WeatherForecast/Get" + queryString);
    }
}