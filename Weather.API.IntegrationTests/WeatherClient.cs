﻿using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Weather.API.IntegrationTests;

public class WeatherClient
{
    private HttpClient _client;

    public WeatherClient(ClientSetup clientSetup)
    {
        //var clientSetup = new ClientSetup();
        _client = clientSetup.Client;
    }

    public async Task<IEnumerable<WeatherForecast>?> GetAllWeathers()
    {
        return await _client.GetFromJsonAsync<IEnumerable<WeatherForecast>>("WeatherForecast/GetAll");
    }

    public async Task AddWeather(IntegrationTests.Models.WeatherForecast forecast)
    {
        await _client.PostAsJsonAsync("WeatherForecast/Add", forecast);
    }


}