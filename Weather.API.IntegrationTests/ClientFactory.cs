using System.Threading.Tasks;
using Dapper;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection.Extensions;
using TryTestContiners.Repository.DbConnection;
using Weather.API.Repository.DbConnection;
using Xunit;

namespace Weather.API.IntegrationTests;

public class ClientFactory : WebApplicationFactory<IApiMarker>, IAsyncLifetime
{
    private static readonly ImageFromDockerfileBuilder _image = new();

    private IDbConnectionFactory _dbConnectionFactory = new DbConnectionFactory(
        "Server=localhost;Initial Catalog=WeatherDatabase;User Id=sa;Password=Passw0rd!!;TrustServerCertificate=true;");
    
    // Need to figure out how to get the docker file into the bin dir of the tests.
    private readonly TestcontainersContainer _testcontainersContainer =
        new TestcontainersBuilder<TestcontainersContainer>()
            .WithImage(_image
                .WithDockerfileDirectory(CommonDirectoryPath.BuildRoot, "../Weather.Database")
                .WithBuildArgument("PASSWORD", "Passw0rd!!")
                .Build()
                .GetAwaiter()
                .GetResult())
            .WithPortBinding(1433, 1433)
            .WithName("mydatabase")
            .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(1433))
            .Build();

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