using BoDi;
using TechTalk.SpecFlow;

namespace Weather.API.IntegrationTests.SF.SystemSetup;

[Binding]
public static class ClientBuilder
{

    private static readonly ClientFactory _clientFactory = new();
    
    [BeforeTestRun]
    public static void StoreClientInContext(IObjectContainer objectContainer)
    {
        var client = _clientFactory.GetClient();

        var weatherClient = new WeatherClient(client);
        
        objectContainer.RegisterInstanceAs(weatherClient);
    }

    [BeforeFeature]
    public static async Task StartContainerAsync()
    {
        await _clientFactory.StartContainerAsync();
    }

    [AfterFeature]
    public static async Task StopContainerAsync()
    {
        await _clientFactory.StopContainerAsync();
    }
}