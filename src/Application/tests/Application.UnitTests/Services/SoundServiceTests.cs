using CFlat.Application.Services;
using CFlat.Core.Constants;

namespace CFlat.Application.UnitTests.Services;

[Collection("Sequential")]
public class SoundServiceTests
{
    [Fact]
    public void LoadAndPlaySound_ShouldNotThrow()
    {
        EngineService.InitializeEngine();

        SoundService.LoadAndPlaySound(ChannelConstants.SoundEffectsChannelGroup, "piano", "D3");

        Assert.True(EngineService.IsRunning);
    }

    [Fact]
    public void LoadAndPlayStream_ShouldNotThrow()
    {
        EngineService.InitializeEngine();

        SoundService.LoadAndPlayStream(ChannelConstants.SoundEffectsChannelGroup, "piano", "D3");

        Assert.True(EngineService.IsRunning);
    }

    [Fact]
    public void LoadAndPlayStream_ShouldPlayStream()
    {
        EngineService.InitializeEngine();

        SoundService.LoadAndPlayStream(ChannelConstants.SoundEffectsChannelGroup, "piano", "D3");
        Thread.Sleep(2000);

        Assert.True(EngineService.IsRunning);
    }

    [Fact]
    public void LoadAndPlay3dStream_ShouldPlay3dStream()
    {
        EngineService.InitializeEngine(true);

        SoundService.LoadAndPlayStream(ChannelConstants.SoundEffectsChannelGroup, "piano", "D3");
        Thread.Sleep(2000);

        Assert.True(EngineService.IsRunning);
    }
}
