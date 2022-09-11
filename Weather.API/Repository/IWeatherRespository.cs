using Weather.API.Models;

namespace Weather.API.Repository;

public interface IWeatherRepository
{
    Task<int> AddWeather(WeatherForecast forecast);
    Task<IEnumerable<WeatherForecast>> GetAll();
    Task<WeatherForecast> Get(DateTime date, int temperatureC);
}