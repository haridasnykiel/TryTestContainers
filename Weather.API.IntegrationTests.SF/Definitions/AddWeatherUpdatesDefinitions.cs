using FluentAssertions;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using Weather.API.IntegrationTests.SF.Models;
using Weather.API.IntegrationTests.SF.SystemSetup;

namespace Weather.API.IntegrationTests.SF.Definitions;

[Binding]
public sealed class AddWeatherUpdatesDefinitions
{
    private WeatherClient _weatherClient;
    private ScenarioContext _context;

    public AddWeatherUpdatesDefinitions(WeatherClient weatherClient, ScenarioContext context)
    {
        _weatherClient = weatherClient;
        _context = context;
    }

    [Given(@"a weather update has been made with the following details")]
    public async Task GivenAWeatherUpdateHasBeenMadeWithTheFollowingDetails(Table table)
    {
        var tables = table.CreateSet<WeatherForecast>();

        foreach (var weatherForecast in tables)
        {
            var addWeatherForecast = new AddWeatherForecast
            {
                Date = weatherForecast.Date,
                TemperatureC = weatherForecast.TemperatureC,
                Summary = weatherForecast.Summary
            };

            await _weatherClient.AddWeatherUpdateAsync(addWeatherForecast);
        }
    }

    [When(@"a request for all weather updates have been made")]
    public async Task WhenARequestForAllWeatherUpdatesHaveBeenMade()
    {
        var response = await _weatherClient.GetAllWeatherUpdates();
        
        _context.Add("forecasts", response);
    }


    [Then(@"the new weather update is included with:")]
    public void ThenTheNewWeatherUpdateIsIncludedWith(Table table)
    {
        var expected = table.CreateSet<WeatherForecast>();
        var isFound = _context.TryGetValue("forecasts", out var actual);

        if (!isFound)
        {
            false.Should().BeTrue();
        }

        var actualResults = actual as IEnumerable<WeatherForecast>;

        foreach (var actualResult in actualResults)
        {
            var result = expected.FirstOrDefault(x =>
                x.Date == actualResult.Date && 
                x.TemperatureC == actualResult.TemperatureC &&
                x.Summary == actualResult.Summary);

            result.Should().NotBeNull();
        }
    }
}
