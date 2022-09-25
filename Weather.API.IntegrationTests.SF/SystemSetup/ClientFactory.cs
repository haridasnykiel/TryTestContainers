using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection.Extensions;
using TechTalk.SpecFlow;
using Weather.API.Repository.DbConnection;

namespace Weather.API.IntegrationTests.SF.SystemSetup;

[Binding]
public class ClientFactory : WebApplicationFactory<IApiMarker>
{
    private readonly IDbConnectionFactory _dbConnectionFactory;
    
    // Need to figure out how to get the docker file into the bin dir of the tests.
    private readonly TestcontainersContainer _testcontainersContainer;

    public ClientFactory()
    {
        var port = new Port();

        _dbConnectionFactory = new DbConnectionFactory(
            $"Server=localhost,{port.Number};Initial Catalog=WeatherDatabase;User Id=sa;Password=Passw0rd!!;TrustServerCertificate=true;");

        var image = Image.Get();
        
        _testcontainersContainer =
            new TestcontainersBuilder<TestcontainersContainer>()
                .WithImage(image)
                .WithPortBinding(port.Number, 1433)
                .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(1433))
                .Build();
    }
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(x =>
        {
            x.RemoveAll(typeof(IDbConnectionFactory));
            x.TryAddSingleton(_dbConnectionFactory);
        });
        
        base.ConfigureWebHost(builder);
    }
    
    public HttpClient GetClient()
    {
        return CreateClient();
    }
    
    public async Task StartContainerAsync()
    {
        await _testcontainersContainer.StartAsync();
    }
    
    public async Task StopContainerAsync()
    {
        await _testcontainersContainer.StopAsync();
    }
}