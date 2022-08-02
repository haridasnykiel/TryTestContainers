namespace Weather.API.Repository;

public interface IWeatherRepository
{
    Task AddWeather(WeatherForecast forecast);
    Task<IEnumerable<WeatherForecast>> GetAll();
}