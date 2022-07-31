using System.Data.SqlClient;
using Dapper;

namespace Weather.API.Repository;

public class WeatherRepository
{
    private readonly Config _config;
    
    public WeatherRepository(Config config)
    {
        _config = config;
    }

    public async Task AddWeather(WeatherForecast forecast)
    {
        await using var connection = new SqlConnection(_config.ConnectionString);

        var insert =
            $"insert into Weather (Date, TemperatureC, Summary) values ('{forecast.Date:s}', {forecast.TemperatureC}, '{forecast.Summary}')";
        
        await connection.ExecuteAsync(insert);
    }
    
    // need a database project
    public async Task<IEnumerable<WeatherForecast>> GetAll()
    {
        using var connection = new SqlConnection(_config.ConnectionString);

        var results =  await connection.QueryAsync<WeatherForecast>("select * from Weather");

        return results;
    }
}