using System;
using System.Runtime.InteropServices;
using AridityTeam.Platform.Diagnostics.Platform;

namespace AridityTeam.Platform.Tests.Platform;

public class PlatformInfoTests
{
    [Fact]
    public void OperatingSystem_ShouldNotBeEmpty()
    {
        var os = PlatformInfo.OperatingSystem;

        Assert.NotNull(os);
        Assert.NotEmpty(os);
    }

    [Fact]
    public void ProcessArchitecture_ShouldBeValid()
    {
        var architecture = PlatformInfo.ProcessArchitecture;

        Assert.True(Enum.IsDefined(typeof(Architecture), architecture));
    }

    [Fact]
    public void FrameworkDescription_ShouldNotBeEmpty()
    {
        var framework = PlatformInfo.FrameworkDescription;

        Assert.NotNull(framework);
        Assert.NotEmpty(framework);
    }

    [Fact]
    public void RuntimeVersion_ShouldNotBeNull()
    {
        var version = PlatformInfo.RuntimeVersion;

        Assert.NotNull(version);
    }

    [Fact]
    public void PlatformDetection_ShouldBeConsistent()
    {
        var isWindows = PlatformInfo.IsWindows;
        var isLinux = PlatformInfo.IsLinux;
        var isMacOS = PlatformInfo.IsMacOS;

        var platformCount = (isWindows ? 1 : 0) + (isLinux ? 1 : 0) + (isMacOS ? 1 : 0);
        Assert.Equal(1, platformCount);
    }

    [Fact]
    public void FrameworkDetection_ShouldBeConsistent()
    {
        var isNetFramework = PlatformInfo.IsNetFramework;
        var isNetCoreOrNet5Plus = PlatformInfo.IsNetCoreOrNet5Plus;

        Assert.True(isNetFramework ^ isNetCoreOrNet5Plus);
    }
} 