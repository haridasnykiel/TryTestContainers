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

    public ClientFactory(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
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
}