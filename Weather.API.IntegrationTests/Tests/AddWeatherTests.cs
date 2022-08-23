using System.Collections.Generic;
using System.Threading.Tasks;
using Bogus;
using FluentAssertions;
using Xunit;

namespace Weather.API.IntegrationTests.Tests;

public class AddWeatherTests : IClassFixture<ClientFactory>
{
    private readonly Faker<IntegrationTests.Models.WeatherForecast> _faker;
    private readonly WeatherClient _client;
    private readonly ClientFactory _clientFactory;
    public AddWeatherTests(ClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
        _faker = new Faker<Models.WeatherForecast>().Rules((r, f) =>
        {
            f.TemperatureC = r.System.Random.Int();
            f.Date = r.Date.Future().Date;
            f.Summary = r.Lorem.Locale;
        });
        
        _client = new WeatherClient(clientFactory.CreateClient());
    }

    [Fact]
    public async Task AddOneWeatherUpdate()
    {
        var forecast = _faker.Generate();
        var expected = new List<IntegrationTests.Models.WeatherForecast> { forecast };

        await _client.AddWeather(forecast);

        var result = await _client.GetAllWeathers();

        result.Should().NotBeEmpty();
        result.Should().BeEquivalentTo(expected);

        await _clientFactory.ClearDatabase();
    }
    
    [Fact]
    public async Task AddMultipleWeatherUpdates()
    {
        var forecastFirst = _faker.Generate();
        var forecastSecond = _faker.Generate();
        var expected = new List<IntegrationTests.Models.WeatherForecast> {forecastFirst, forecastSecond};

        await _client.AddWeather(forecastFirst);
        await _client.AddWeather(forecastSecond);

        var result = await _client.GetAllWeathers();

        result.Should().NotBeEmpty();
        result.Should().BeEquivalentTo(expected);
        
        await _clientFactory.ClearDatabase();
    }
}