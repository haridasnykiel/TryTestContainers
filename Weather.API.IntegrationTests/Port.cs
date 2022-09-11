using System;

namespace Weather.API.IntegrationTests;

public class Port
{
    private readonly int _port;

    public Port()
    {
        _port = new Random().Next(8000, 9000);
    }

    public int Number => _port;
}