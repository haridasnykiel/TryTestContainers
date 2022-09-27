using BoDi;
using DotNet.Testcontainers.Containers;
using TechTalk.SpecFlow;

namespace Weather.API.IntegrationTests.SF.SystemSetup;

[Binding]
public static class TestBuilder
{
    private static DbContainerFactory? _dbContainerFactory;

    [BeforeTestRun]
    public static void InitDbContainerFactory()
    {
        _dbContainerFactory = new DbContainerFactory();
    }

    [BeforeFeature]
    public static async Task BuildAndStartContainerAsync(IObjectContainer objectContainer, FeatureContext featureContext)
    {
        var dbConnectionFactoryPortNumber = _dbContainerFactory.BuildDbContainer();
        var containerAndDbConnectionFactory = _dbContainerFactory.GetContainerAndDbConnectionFactory(dbConnectionFactoryPortNumber);
        
        var client = new ClientFactory(containerAndDbConnectionFactory.Item1);

        var weatherClient = new WeatherClient(client.CreateClient());
        
        objectContainer.RegisterInstanceAs(weatherClient, featureContext.FeatureInfo.Title);
        
        featureContext.Add(featureContext.FeatureInfo.Title, containerAndDbConnectionFactory.Item2);

        await containerAndDbConnectionFactory.Item2.StartAsync();
    }

    [AfterFeature]
    public static async Task StopContainerAsync(FeatureContext featureContext)
    {
        featureContext.TryGetValue<TestcontainersContainer>(featureContext.FeatureInfo.Title, out var container);
        await container.StopAsync();
    }
}