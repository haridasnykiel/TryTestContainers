using System.Configuration;
using System.Threading.Tasks;
using Bogus;
using FluentAssertions;
using Xunit;

namespace Weather.API.IntegrationTests.Tests;

public class AddWeatherTests
{
    private readonly Faker<IntegrationTests.Models.WeatherForecast> _faker;
    private readonly WeatherClient _client;
    public AddWeatherTests(ClientFactory clientFactory)
    {
        _faker = new Faker<Models.WeatherForecast>();
        _client = new WeatherClient(clientFactory.CreateClient());
    }

    [Fact]
    public async Task AddOneWeatherUpdate()
    {
        var forecast = _faker.Generate();

        await _client.AddWeather(forecast);

        var result = await _client.GetAllWeathers();

        result.Should().NotBeEmpty();
    }
}