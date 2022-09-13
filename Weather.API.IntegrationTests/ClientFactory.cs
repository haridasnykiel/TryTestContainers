using System.Threading.Tasks;
using Dapper;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Weather.API.Repository.DbConnection;
using Xunit;

namespace Weather.API.IntegrationTests;

public class ClientFactory : WebApplicationFactory<IApiMarker>, IAsyncLifetime
{
    private readonly IDbConnectionFactory _dbConnectionFactory;
    
    // Need to figure out how to get the docker file into the bin dir of the tests.
    private readonly TestcontainersContainer _testcontainersContainer;

    public ClientFactory()
    {
        var port = new Port();
        var imageFromDockerfileBuilder = new ImageFromDockerfileBuilder();
        
        _dbConnectionFactory = new DbConnectionFactory(
            $"Server=localhost,{port.Number};Initial Catalog=WeatherDatabase;User Id=sa;Password=Passw0rd!!;TrustServerCertificate=true;");
        
        var image = imageFromDockerfileBuilder
            .WithDockerfileDirectory("../../../../Weather.Database")
            .WithBuildArgument("PASSWORD", "Passw0rd!!")
            .Build()
            .GetAwaiter()
            .GetResult();
        
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

    public async Task ClearDatabase()
    {
        using var connection = _dbConnectionFactory.Create();
        await connection.ExecuteAsync("delete from Weather");
    }
    
    public async Task InitializeAsync()
    {
        await _testcontainersContainer.StartAsync();
    }

    public new async Task DisposeAsync()
    {
        await _testcontainersContainer.DisposeAsync();
    }
}