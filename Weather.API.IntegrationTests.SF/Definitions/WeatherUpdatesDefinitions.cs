using BoDi;
using FluentAssertions;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using Weather.API.IntegrationTests.SF.Models;
using Weather.API.IntegrationTests.SF.SystemSetup;

namespace Weather.API.IntegrationTests.SF.Definitions;

[Binding]
public sealed class WeatherUpdatesDefinitions
{
    private readonly WeatherClient _weatherClient;
    private readonly ScenarioContext _context;

    public WeatherUpdatesDefinitions(IObjectContainer objectContainer, FeatureContext featureContext, ScenarioContext context)
    {
        _weatherClient = objectContainer.Resolve<WeatherClient>(featureContext.FeatureInfo.Title);
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
        var expectedResults = table.CreateSet<WeatherForecast>();
        var isFound = _context.TryGetValue("forecasts", out var actual);

        if (!isFound)
        {
            false.Should().BeTrue();
        }

        var actualResults = actual as IEnumerable<WeatherForecast>;

        foreach (var expectedResult in expectedResults)
        {
            var actualResult = actualResults.FirstOrDefault(x =>
                x.Date == expectedResult.Date && 
                x.TemperatureC == expectedResult.TemperatureC);

            actualResult.Should().NotBeNull();
            actualResult.Summary.Should().Be(expectedResult.Summary);
        }
    }

    [When(@"a request for a weather update with:")]
    public async Task WhenARequestForAWeatherUpdateWith(Table table)
    {
        var request = table.CreateInstance<GetWeatherForecast>();

        var response = await _weatherClient.GetWeatherUpdate(request);
        _context.Add("forecasts", new List<WeatherForecast>{response});
    }
}
