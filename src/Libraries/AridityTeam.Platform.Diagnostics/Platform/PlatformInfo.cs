using System;
using System.Runtime.InteropServices;

namespace AridityTeam.Platform;

/// <summary>
/// Provides information about the current platform and runtime environment.
/// </summary>
public static class PlatformInfo
{
    /// <summary>
    /// Gets a value indicating whether the current platform is Windows.
    /// </summary>
    public static bool IsWindows => RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

    /// <summary>
    /// Gets a value indicating whether the current platform is Linux.
    /// </summary>
    public static bool IsLinux => RuntimeInformation.IsOSPlatform(OSPlatform.Linux);

    /// <summary>
    /// Gets a value indicating whether the current platform is macOS.
    /// </summary>
    public static bool IsMacOS => RuntimeInformation.IsOSPlatform(OSPlatform.OSX);

    /// <summary>
    /// Gets the current operating system platform.
    /// </summary>
    public static string OperatingSystem => RuntimeInformation.OSDescription;

    /// <summary>
    /// Gets the current process architecture.
    /// </summary>
    public static Architecture ProcessArchitecture => RuntimeInformation.ProcessArchitecture;

    /// <summary>
    /// Gets the current runtime framework description.
    /// </summary>
    
    public static string FrameworkDescription => RuntimeInformation.FrameworkDescription;

    /// <summary>
    /// Gets a value indicating whether the current runtime is .NET Framework.
    /// </summary>
    public static bool IsNetFramework => FrameworkDescription.StartsWith(".NET Framework", StringComparison.OrdinalIgnoreCase);

    /// <summary>
    /// Gets a value indicating whether the current runtime is .NET Core or .NET 5+.
    /// </summary>
    public static bool IsNetCoreOrNet5Plus => FrameworkDescription.StartsWith(".NET", StringComparison.OrdinalIgnoreCase) && !IsNetFramework;

    /// <summary>
    /// Gets the current runtime version.
    /// </summary>
    public static Version RuntimeVersion => Environment.Version;
}