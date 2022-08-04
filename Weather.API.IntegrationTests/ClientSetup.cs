using System.Net.Http;
using System.Threading.Tasks;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Weather.API.Repository.DbConnection;
using Xunit;

namespace Weather.API.IntegrationTests;

public class ClientSetup : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly TestcontainersContainer _testcontainersContainer =
        new TestcontainersBuilder<MsSqlTestcontainer>()
            .WithDatabase(new MsSqlTestcontainerConfiguration
            {
                Password = "Test1!!"
            })
            .WithPortBinding(1433, 1433)
            .Build();

    public ClientSetup()
    {
        WithWebHostBuilder(ConfigureWebHost);
        Client = CreateClient();
    }
    
    public HttpClient Client { get; }
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(x =>
        {
            x.RemoveAll(typeof(IDbConnectionFactory));
            x.TryAddSingleton<IDbConnectionFactory>(
                new DbConnectionFactory("Server=localhost;Initial Catalog=TestWeatherDatabase;User Id=sa;Password=Test1!!;TrustServerCertificate=true;"));
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