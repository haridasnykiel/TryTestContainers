

using DotNet.Testcontainers.Builders;

namespace Weather.API.IntegrationTests.SF.SystemSetup;

public static class Image
{
    private static readonly ImageFromDockerfileBuilder _builder = new();
    private static string _image;
    private static readonly SemaphoreSlim _semaphoreSlim = new(1, 1);

    public static async Task<string> GetAsync()
    {
        await _semaphoreSlim.WaitAsync();

        try
        {
            if (string.IsNullOrEmpty(_image))
            {
                _image = await _builder
                    .WithDockerfileDirectory("../../../../Weather.Database")
                    .WithBuildArgument("PASSWORD", "Passw0rd!!")
                    .WithDeleteIfExists(true)
                    .Build();
            }
        }
        finally
        {
            _semaphoreSlim.Release();
        }

        return _image;
    }
}