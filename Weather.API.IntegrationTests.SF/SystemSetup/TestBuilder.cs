using BoDi;
using DotNet.Testcontainers.Containers;
using TechTalk.SpecFlow;

namespace Weather.API.IntegrationTests.SF.SystemSetup;

[Binding]
public static class TestBuilder
{
    [BeforeFeature]
    public static async Task BuildAndStartContainerAsync(IObjectContainer objectContainer, FeatureContext featureContext)
    {
        var dbContainerFactory = new DbContainerFactory();
        var dbConnectionFactoryPortNumber = dbContainerFactory.BuildDbContainer();
        
        var containerAndDbConnectionFactory = dbContainerFactory
            .GetContainerAndDbConnectionFactory(dbConnectionFactoryPortNumber);
        
        var client = new ClientFactory(containerAndDbConnectionFactory.Item1);

        var weatherClient = new WeatherClient(client.GetClient());
        
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