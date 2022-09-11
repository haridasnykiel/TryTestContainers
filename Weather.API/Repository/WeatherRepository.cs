using Dapper;
using Weather.API.Models;
using Weather.API.Repository.DbConnection;

namespace Weather.API.Repository;

public class WeatherRepository : IWeatherRepository
{
    private readonly IDbConnectionFactory _dbConnectionFactory;
    
    public WeatherRepository(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<int> AddWeather(WeatherForecast forecast)
    {
        using var connection = _dbConnectionFactory.Create();

        var insert =
            "insert into Weather (Date, TemperatureC, Summary) " +
            $"values ('{forecast.Date:s}', {forecast.TemperatureC}, '{forecast.Summary}')";
        
        return await connection.ExecuteAsync(insert);
    }

    public async Task<IEnumerable<WeatherForecast>> GetAll()
    {
        using var connection = _dbConnectionFactory.Create();

        var results =  await connection.QueryAsync<WeatherForecast>("select * from Weather");

        return results;
    }
    
    public async Task<WeatherForecast> Get(DateTime date, int temperatureC)
    {
        using var connection = _dbConnectionFactory.Create();

        var sql = @"select
        Id,
        Date,
        TemperatureC,
        Summary
        from Weather " +
                  $"where Date = \'{date.Date:s}\' " +
                  $"and TemperatureC = {temperatureC}";

        var result =  await connection.QueryFirstAsync<WeatherForecast>(sql);

        return result;
    }
}