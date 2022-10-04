

using DotNet.Testcontainers.Builders;

namespace Weather.API.IntegrationTests.SF.SystemSetup;

public static class Image
{
    private static readonly ImageFromDockerfileBuilder _builder = new();
    private static string _image;

    public static async Task<string> GetAsync()
    {
        if (string.IsNullOrEmpty(_image))
        {
            _image = await _builder
                .WithDockerfileDirectory("../../../../Weather.Database")
                .WithBuildArgument("PASSWORD", "Passw0rd!!")
                .WithDeleteIfExists(true)
                .Build();
        }
        
        return _image;
    }
}