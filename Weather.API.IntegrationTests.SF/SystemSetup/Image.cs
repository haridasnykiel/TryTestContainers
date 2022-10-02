

using DotNet.Testcontainers.Builders;

namespace Weather.API.IntegrationTests.SF.SystemSetup;

public static class Image
{
    private static readonly ImageFromDockerfileBuilder _builder = new();
    private static readonly string _image;

    static Image()
    {
        _image = _builder
            .WithDockerfileDirectory("../../../../Weather.Database")
            .WithBuildArgument("PASSWORD", "Passw0rd!!")
            .WithDeleteIfExists(true)
            .Build()
            .GetAwaiter()
            .GetResult();
    }

    public static string Get()
    {
        return _image;
    }
}