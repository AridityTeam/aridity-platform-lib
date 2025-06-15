using System;
using System.Diagnostics;
using System.Threading;

namespace AridityTeam.Diagnostics;

/// <summary>
/// Provides utilities for monitoring performance metrics.
/// </summary>
public static class PerformanceMonitor
{
    /// <summary>
    /// Measures the execution time of an action.
    /// </summary>
    /// <param name="action">The action to measure.</param>
    /// <returns>The elapsed time in milliseconds.</returns>
    public static long MeasureExecutionTime(Action action)
    {
        var stopwatch = Stopwatch.StartNew();
        action();
        stopwatch.Stop();
        return stopwatch.ElapsedMilliseconds;
    }

    /// <summary>
    /// Measures the execution time of a function and returns its result.
    /// </summary>
    /// <typeparam name="T">The return type of the function.</typeparam>
    /// <param name="func">The function to measure.</param>
    /// <param name="elapsedMilliseconds">The elapsed time in milliseconds.</param>
    /// <returns>The result of the function.</returns>
    public static T MeasureExecutionTime<T>(Func<T> func, out long elapsedMilliseconds)
    {
        var stopwatch = Stopwatch.StartNew();
        var result = func();
        stopwatch.Stop();
        elapsedMilliseconds = stopwatch.ElapsedMilliseconds;
        return result;
    }

    /// <summary>
    /// Gets the current memory usage of the process in megabytes.
    /// </summary>
    /// <returns>The memory usage in megabytes.</returns>
    public static double GetCurrentMemoryUsage()
    {
        var process = Process.GetCurrentProcess();
        return process.PrivateMemorySize64 / (1024.0 * 1024.0);
    }

    /// <summary>
    /// Gets the peak memory usage of the process in megabytes.
    /// </summary>
    /// <returns>The peak memory usage in megabytes.</returns>
    public static double GetPeakMemoryUsage()
    {
        var process = Process.GetCurrentProcess();
        return process.PeakWorkingSet64 / (1024.0 * 1024.0);
    }

    /// <summary>
    /// Measures the memory usage before and after an action.
    /// </summary>
    /// <param name="action">The action to measure.</param>
    /// <returns>A tuple containing the initial and final memory usage in megabytes.</returns>
    public static (double InitialMB, double FinalMB) MeasureMemoryUsage(Action action)
    {
        // Get initial memory
        var initialMemory = GetCurrentMemoryUsage();

        // Run the action
        action();

        // Get final memory
        var finalMemory = GetCurrentMemoryUsage();

        // If final memory is less than initial, it might be due to GC
        // In this case, we'll use the peak memory instead
        if (finalMemory < initialMemory)
        {
            finalMemory = GetPeakMemoryUsage();
        }

        return (initialMemory, finalMemory);
    }

    /// <summary>
    /// Creates a performance scope that measures execution time and memory usage.
    /// </summary>
    /// <returns>A disposable performance scope.</returns>
    public static IDisposable CreatePerformanceScope()
    {
        return new PerformanceScope();
    }

    private class PerformanceScope : IDisposable
    {
        private readonly Stopwatch _stopwatch;
        private readonly double _initialMemory;
        private readonly string _callerMemberName;
        private readonly string _callerFilePath;
        private readonly int _callerLineNumber;

        public PerformanceScope(
            [System.Runtime.CompilerServices.CallerMemberName] string callerMemberName = "",
            [System.Runtime.CompilerServices.CallerFilePath] string callerFilePath = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int callerLineNumber = 0)
        {
            _callerMemberName = callerMemberName;
            _callerFilePath = callerFilePath;
            _callerLineNumber = callerLineNumber;
            _stopwatch = Stopwatch.StartNew();
            _initialMemory = GetCurrentMemoryUsage();
        }

        public void Dispose()
        {
            _stopwatch.Stop();
            var finalMemory = GetCurrentMemoryUsage();
            var memoryDiff = finalMemory - _initialMemory;

            Debug.WriteLine($"Performance Scope: {_callerMemberName} ({_callerFilePath}:{_callerLineNumber})");
            Debug.WriteLine($"  Execution Time: {_stopwatch.ElapsedMilliseconds}ms");
            Debug.WriteLine($"  Memory Usage: {memoryDiff:F2}MB");
        }
    }
}
