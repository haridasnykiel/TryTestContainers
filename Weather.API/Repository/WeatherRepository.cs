using System.Data.SqlClient;
using Dapper;
using Weather.API.Repository.DbConnection;

namespace Weather.API.Repository;

public class WeatherRepository : IWeatherRepository
{
    private readonly IDbConnectionFactory _dbConnectionFactory;
    
    public WeatherRepository(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task AddWeather(WeatherForecast forecast)
    {
        using var connection = _dbConnectionFactory.Create();

        var insert =
            $"insert into Weather (Date, TemperatureC, Summary) values ('{forecast.Date:s}', {forecast.TemperatureC}, '{forecast.Summary}')";
        
        await connection.ExecuteAsync(insert);
    }
    
    // need a database project
    public async Task<IEnumerable<WeatherForecast>> GetAll()
    {
        using var connection = _dbConnectionFactory.Create();

        var results =  await connection.QueryAsync<WeatherForecast>("select * from Weather");

        return results;
    }
}