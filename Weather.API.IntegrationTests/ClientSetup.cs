using System.Data.SqlClient;
using System.Threading.Tasks;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Weather.API.Repository.DbConnection;
using Xunit;

namespace Weather.API.IntegrationTests;

public class ClientSetup : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly TestcontainersContainer _testcontainersContainer =
        new TestcontainersBuilder<TestcontainersContainer>()
            .WithImage("mcr.microsoft.com/mssql/server:2019-latest")
            //.WithEnvironment()
            .Build();
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

    public Task InitializeAsync()
    {
        return Task.CompletedTask;
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }
}