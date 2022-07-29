namespace Weather.API;

public class Config
{
    private IConfiguration _configuration;
    
    public Config(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string ConnectionString => _configuration.GetConnectionString("Database");
}