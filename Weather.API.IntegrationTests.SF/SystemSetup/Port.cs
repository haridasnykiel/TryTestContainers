namespace Weather.API.IntegrationTests.SF.SystemSetup;

public class Port
{
    private readonly int _port;

    public Port()
    {
        _port = new Random().Next(8081, 9000);
    }

    public int Number => _port;
}