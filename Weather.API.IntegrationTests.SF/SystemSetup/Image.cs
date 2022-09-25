

using DotNet.Testcontainers.Builders;

namespace Weather.API.IntegrationTests.SF.SystemSetup;

public static class Image
{
    private static ImageFromDockerfileBuilder _builder = new ImageFromDockerfileBuilder();
    private static string _image;

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