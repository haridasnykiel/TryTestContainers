using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Xunit;

namespace Weather.API.IntegrationTests;

public class ClientSetup : WebApplicationFactory<Program>, IAsyncLifetime
{

    /*protected override void ConfigureWebHost()
    {
        
    }*/
    public Task InitializeAsync()
    {
        return Task.CompletedTask;
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }
}