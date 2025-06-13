using System;
using System.Threading;
using AridityTeam.Platform.Diagnostics.Diagnostics;
using System.Collections.Generic;

namespace AridityTeam.Platform.Tests.Diagnostics;

public class PerformanceMonitorTests
{
    [Fact]
    public void MeasureExecutionTime_ShouldMeasureCorrectly()
    {
        const int expectedDelay = 100;

        var executionTime = PerformanceMonitor.MeasureExecutionTime(() =>
        {
            Thread.Sleep(expectedDelay);
        });

        Assert.True(executionTime >= expectedDelay, $"Execution time {executionTime}ms should be at least {expectedDelay}ms");
    }

    [Fact]
    public void MeasureExecutionTime_WithReturnValue_ShouldMeasureCorrectly()
    {
        const int expectedDelay = 100;
        const int expectedResult = 42;

        var result = PerformanceMonitor.MeasureExecutionTime(() =>
        {
            Thread.Sleep(expectedDelay);
            return expectedResult;
        }, out var executionTime);

        Assert.Equal(expectedResult, result);
        Assert.True(executionTime >= expectedDelay, $"Execution time {executionTime}ms should be at least {expectedDelay}ms");
    }

    [Fact]
    public void GetCurrentMemoryUsage_ShouldReturnPositiveValue()
    {
        var memoryUsage = PerformanceMonitor.GetCurrentMemoryUsage();

        Assert.True(memoryUsage > 0, "Memory usage should be greater than 0");
    }

    [Fact]
    public void GetPeakMemoryUsage_ShouldReturnPositiveValue()
    {
        var peakMemoryUsage = PerformanceMonitor.GetPeakMemoryUsage();

        Assert.True(peakMemoryUsage > 0, "Peak memory usage should be greater than 0");
    }

    [Fact]
    public void MeasureMemoryUsage_ShouldDetectMemoryAllocation()
    {
        const int arraySize = 10000000; // 10MB per array
        var arrays = new List<byte[]>();
        var random = new Random();
        
        var peakBefore = PerformanceMonitor.GetPeakMemoryUsage();

        var (initialMemory, finalMemory) = PerformanceMonitor.MeasureMemoryUsage(() =>
        {
            for (int i = 0; i < 10; i++)
            {
                var array = new byte[arraySize];
                random.NextBytes(array);
                arrays.Add(array);
            }
            GC.KeepAlive(arrays);
        });

        var peakAfter = PerformanceMonitor.GetPeakMemoryUsage();

        Assert.True(peakAfter > peakBefore, $"Peak memory after allocation ({peakAfter:F2}MB) should be greater than before ({peakBefore:F2}MB)");
    }

    [Fact]
    public void CreatePerformanceScope_ShouldMeasureExecution()
    {
        const int expectedDelay = 100;
        var scope = PerformanceMonitor.CreatePerformanceScope();

        Thread.Sleep(expectedDelay);
        scope.Dispose();
    }
} 