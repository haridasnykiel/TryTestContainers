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

        var id = await connection.QueryAsync<int>("select Id from WeatherDetails");

        var max = id.Max() + 1;

        var insert =
            $"insert into WeatherDetails (Id, Date, TemperatureC, Summary) values ({max}, '{forecast.Date:s}', {forecast.TemperatureC}, '{forecast.Summary}')";
        
        await connection.ExecuteAsync(insert);
    }
    
    // need a database project
    public async Task<IEnumerable<WeatherForecast>> GetAll()
    {
        using var connection = new SqlConnection(_config.ConnectionString);

        var results =  await connection.QueryAsync<WeatherForecast>("select * from weather");

        return results;
    }
}