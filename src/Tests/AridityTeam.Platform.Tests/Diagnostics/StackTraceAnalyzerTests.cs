using System;
using System.Linq;
using AridityTeam.Diagnostics;
using System.Diagnostics;

namespace AridityTeam.Platform.Tests.Diagnostics;

public class StackTraceAnalyzerTests
{
    [Fact]
    public void GetFormattedStackTrace_ShouldNotBeEmpty()
    {
        var stackTrace = StackTraceAnalyzer.GetFormattedStackTrace();

        Assert.NotNull(stackTrace);
        Assert.NotEmpty(stackTrace);
    }

    [Fact]
    public void GetFormattedExceptionStackTrace_ShouldFormatExceptionStackTrace()
    {
        var exception = new Exception("Test exception");

        var stackTrace = StackTraceAnalyzer.GetFormattedExceptionStackTrace(exception);

        Assert.NotNull(stackTrace);
        Assert.NotEmpty(stackTrace);
        Assert.Contains("System.Exception", stackTrace);
    }

    [Fact]
    public void GetFormattedExceptionStackTrace_WithNullException_ShouldThrowArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => StackTraceAnalyzer.GetFormattedExceptionStackTrace(null!));
    }

    [Fact]
    public void GetCallingMethodInfo_ShouldReturnValidInfo()
    {
        var (methodName, filePath, lineNumber) = GetCallingMethodInfo();

        Assert.NotNull(methodName);
        Assert.NotEmpty(methodName);
        Assert.NotNull(filePath);
        Assert.True(lineNumber > 0);
    }

    [Fact]
    public void GetMethodCallStack_ShouldReturnValidStack()
    {
        var methodCalls = StackTraceAnalyzer.GetMethodCallStack();

        Assert.NotNull(methodCalls);
        Assert.NotEmpty(methodCalls);
        Assert.All(methodCalls, call =>
        {
            Assert.NotNull(call.MethodName);
            Assert.NotNull(call.FilePath);
            Assert.True(call.LineNumber > 0);
        });
    }

    [Fact]
    public void GetMethodCallStack_ShouldIncludeCurrentMethod()
    {
        var methodCalls = StackTraceAnalyzer.GetMethodCallStack();
        var currentMethod = GetCurrentMethodName();

        var allMethodNames = string.Join(", ", methodCalls.Select(c => c.MethodName));

        Assert.True(methodCalls.Any(call =>
            call.MethodName != null &&
            call.MethodName.Contains(currentMethod, StringComparison.Ordinal)), 
            $"Current method: {currentMethod}, All methods in stack: {allMethodNames}");
    }

    private static (string MethodName, string FilePath, int LineNumber) GetCallingMethodInfo()
    {
        return StackTraceAnalyzer.GetCallingMethodInfo();
    }

    private static string GetCurrentMethodName()
    {
        var stackTrace = new StackTrace();
        return stackTrace.GetFrame(1)?.GetMethod()?.Name ?? string.Empty;
    }
} 