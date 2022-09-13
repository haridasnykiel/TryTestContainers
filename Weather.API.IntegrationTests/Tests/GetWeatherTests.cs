using System.Threading.Tasks;
using Bogus;
using FluentAssertions;
using Weather.API.IntegrationTests.Models;
using Xunit;

namespace Weather.API.IntegrationTests.Tests;

public class GetWeatherTests : IClassFixture<ClientFactory>
{
    private readonly Faker<AddWeatherForecast> _faker;
    private readonly WeatherClient _client;

    public GetWeatherTests(ClientFactory clientFactory)
    {
        _faker = new Faker<AddWeatherForecast>().Rules((r, f) =>
        {
            f.TemperatureC = r.System.Random.Int();
            f.Date = r.Date.Future().Date;
            f.Summary = r.Lorem.Locale;
        });
        
        _client = new WeatherClient(clientFactory.CreateClient());
    }

    [Fact]
    public async Task GetAWeatherUpdate()
    {
        var request = _faker.Generate();
        var expected = new WeatherForecast
        {
            Date = request.Date,
            TemperatureC = request.TemperatureC,
            Summary = request.Summary
        };
        
        await _client.AddWeatherUpdate(request);

        var result = await _client.GetWeatherUpdate(new GetWeatherForecast
        {
            Date = request.Date,
            TemperatureC = request.TemperatureC
        });

        result.Should().BeEquivalentTo(expected);
    }
}