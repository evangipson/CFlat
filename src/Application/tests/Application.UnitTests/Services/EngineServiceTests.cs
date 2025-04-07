using CFlat.Application.Services;

namespace CFlat.Application.UnitTests.Services;

[Collection("Sequential")]
public class EngineServiceTests
{
    [Fact]
    public void InitializeEngine_ShouldCreateEngine_AndPopulateChannelGroups()
    {
        EngineService.InitializeEngine();

        Assert.True(EngineService.IsRunning);
        Assert.NotNull(EngineService.AudioEngine?.ChannelGroups);
        Assert.NotEmpty(EngineService.AudioEngine?.ChannelGroups!);
    }

    [Fact]
    public void StopEngine_ShouldMakeEngineStopRunning()
    {
        EngineService.StopEngine();
        Assert.False(EngineService.IsRunning);
    }

    [Fact]
    public void StartAndStopEngine_ShouldNotThrow()
    {
        EngineService.InitializeEngine();
        Assert.True(EngineService.IsRunning);

        EngineService.StopEngine();
        Assert.False(EngineService.IsRunning);

        EngineService.InitializeEngine();
        Assert.True(EngineService.IsRunning);

        EngineService.StopEngine();
        Assert.False(EngineService.IsRunning);
    }

    [Fact]
    public void Initialize3dEngine_ShouldCreateEngine_AndPopulateChannelGroups()
    {
        EngineService.InitializeEngine(true);

        Assert.True(EngineService.IsRunning);
        Assert.NotNull(EngineService.AudioEngine?.ChannelGroups);
        Assert.NotEmpty(EngineService.AudioEngine?.ChannelGroups!);
    }
}
