using System.Collections.Concurrent;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using Weather.API.Repository.DbConnection;

namespace Weather.API.IntegrationTests.SF.SystemSetup;

public class DbContainerFactory
{
    private readonly ConcurrentDictionary<int, (IDbConnectionFactory, TestcontainersContainer)> _dbConnectionFactories = new();

    public int BuildDbContainer()
    {
        var port = new Port();

        var dbConnectionFactory = new DbConnectionFactory(
            $"Server=localhost,{port.Number};Initial Catalog=WeatherDatabase;User Id=sa;Password=Passw0rd!!;TrustServerCertificate=true;");

        var image = Image.Get();

        var testcontainersContainer = new TestcontainersBuilder<TestcontainersContainer>()
            .WithImage(image)
            .WithPortBinding(port.Number, 1433)
            .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(1433))
            .Build();
        
        var isAdded = _dbConnectionFactories.TryAdd(port.Number, (dbConnectionFactory, testcontainersContainer));

        if (!isAdded)
        {
            throw new InvalidOperationException($"Port number has already been used {port.Number}");
        }

        return port.Number;
    }

    public (IDbConnectionFactory, TestcontainersContainer) GetContainerAndDbConnectionFactory(int portNumber)
    {
        if (!_dbConnectionFactories.TryGetValue(portNumber, out var value))
        {
            throw new KeyNotFoundException($"The port number {portNumber} was not found");
        }

        return value;
    }
}