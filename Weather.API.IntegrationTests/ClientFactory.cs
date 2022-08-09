using System.Threading.Tasks;
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
    private static readonly ImageFromDockerfileBuilder _image = new();
    
    // Need to figure out how to get the docker file into the bin dir of the tests.
    private readonly TestcontainersContainer _testcontainersContainer =
        new TestcontainersBuilder<TestcontainersContainer>()
            .WithImage(_image
                .WithDockerfileDirectory("../../../../Weather.Database")
                .WithBuildArgument("PASSWORD", "Passw0rd!!")
                .Build()
                .GetAwaiter()
                .GetResult())
            .WithPortBinding(1433, 1433)
            .WithName("mydatabase")
            .Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(x =>
        {
            x.RemoveAll(typeof(IDbConnectionFactory));
            x.TryAddSingleton<IDbConnectionFactory>(
                new DbConnectionFactory("Server=localhost;Initial Catalog=WeatherDatabase;User Id=sa;Password=Passw0rd!!;TrustServerCertificate=true;"));
        });
        
        base.ConfigureWebHost(builder);
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