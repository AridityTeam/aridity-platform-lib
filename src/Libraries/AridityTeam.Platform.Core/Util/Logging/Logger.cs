/*
 * Copyright (c) 2025 The Aridity Team
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */

using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace AridityTeam.Util.Logging;

/// <summary>
/// A flexible and performant logger implementation.
/// </summary>
public class Logger : ILogger
{
    private readonly string _name;
    private readonly string _logDirectory;
    private readonly ConcurrentQueue<LogEntry> _logQueue;
    private readonly CancellationTokenSource _cancellationTokenSource;
    private readonly Task? _processQueueTask;
    private readonly object _fileLock = new();
    private bool _isDisposed;
    private string? _currentLogFile;
    private DateTime _currentLogDate;
    private int _maxFileSizeMB = 10;
    private int _maxFilesPerDay = 5;

    /// <summary>
    /// Gets or sets whether console colors are enabled.
    /// </summary>
    public bool ConsoleColorsEnabled { get; set; } = true;

    /// <summary>
    /// Gets or sets the console colors for different log levels.
    /// </summary>
    public Dictionary<LogLevel, (ConsoleColor Foreground, ConsoleColor Background)> LogLevelColors { get; } = new()
    {
        { LogLevel.Info, (ConsoleColor.White, ConsoleColor.Black) },
        { LogLevel.Warn, (ConsoleColor.Yellow, ConsoleColor.Black) },
        { LogLevel.Error, (ConsoleColor.Red, ConsoleColor.Black) },
        { LogLevel.Fatal, (ConsoleColor.White, ConsoleColor.Red) }
    };

    /// <summary>
    /// Gets or sets the maximum number of log entries to batch before writing to file.
    /// </summary>
    public int MaxBatchSize { get; set; } = 100;

    /// <summary>
    /// Gets or sets the maximum time to wait before writing batched logs to file (in milliseconds).
    /// </summary>
    public int MaxBatchWaitTime { get; set; } = 1000;

    /// <summary>
    /// Gets or sets the maximum log file size in megabytes.
    /// </summary>
    public int MaxFileSizeMB
    {
        get => _maxFileSizeMB;
        set
        {
            if (value < 1) throw new ArgumentOutOfRangeException(nameof(value), "Max file size must be at least 1MB");
            _maxFileSizeMB = value;
        }
    }

    /// <summary>
    /// Gets or sets the maximum number of log files per day.
    /// </summary>
    public int MaxFilesPerDay
    {
        get => _maxFilesPerDay;
        set
        {
            if (value < 1) throw new ArgumentOutOfRangeException(nameof(value), "Max files per day must be at least 1");
            _maxFilesPerDay = value;
        }
    }

    /// <summary>
    /// Creates a new logger with the specified type.
    /// </summary>
    public static Logger GetLogger<TClass>(bool enableConsoleLogging = true, 
        bool enableFileLogging = false) where TClass : class =>
        GetLogger(typeof(TClass).Name, enableConsoleLogging, enableFileLogging);

    /// <summary>
    /// Creates a new logger with the specified name.
    /// </summary>
    public static Logger GetLogger(string name, bool enableConsoleLogging = true, bool enableFileLogging = false)
    {
        return new Logger(name, enableConsoleLogging, enableFileLogging);
    }

    /// <summary>
    /// Initializes a new instance of the Logger class.
    /// </summary>
    public Logger(string name, bool enableConsoleLogging = true, bool enableFileLogging = false)
    {
        _name = name;
        ConsoleLoggingEnabled = enableConsoleLogging;
        FileLoggingEnabled = enableFileLogging;
        _logDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
        _logQueue = new ConcurrentQueue<LogEntry>();
        _cancellationTokenSource = new CancellationTokenSource();

        if (FileLoggingEnabled)
        {
            Directory.CreateDirectory(_logDirectory);
            _processQueueTask = Task.Run(ProcessLogQueue);
        }
    }

    /// <summary>
    /// Gets or sets whether console logging is enabled.
    /// </summary>
    public bool ConsoleLoggingEnabled { get; set; }

    /// <summary>
    /// Gets or sets whether file logging is enabled.
    /// </summary>
    public bool FileLoggingEnabled { get; set; }

    /// <summary>
    /// Gets or sets the minimum log level.
    /// </summary>
    public LogLevel MinimumLogLevel { get; set; } = LogLevel.Info;

    /// <summary>
    /// Gets or sets the maximum log file age in days.
    /// </summary>
    public int MaxLogFileAge { get; set; } = 30;

    /// <summary>
    /// Logs a message with the specified level.
    /// </summary>
    public void Log(string? message, LogLevel level = LogLevel.Info)
    {
        if (level < MinimumLogLevel) return;

        var entry = new LogEntry
        {
            Timestamp = DateTime.Now,
            Level = level,
            Message = message,
            LoggerName = _name
        };

        if (ConsoleLoggingEnabled)
        {
            ConsoleLog(entry);
        }

        if (FileLoggingEnabled)
        {
            _logQueue.Enqueue(entry);
        }
    }

    /// <summary>
    /// Logs an exception with the specified level.
    /// </summary>
    public void LogException(Exception exception, LogLevel level = LogLevel.Error)
    {
        var message = new StringBuilder();
        message.AppendLine($"Exception: {exception.GetType().Name}");
        message.AppendLine($"Message: {exception.Message}");
        message.AppendLine("Stack Trace:");
        message.AppendLine(exception.StackTrace);

        if (exception.InnerException != null)
        {
            message.AppendLine("Inner Exception:");
            message.AppendLine($"Type: {exception.InnerException.GetType().Name}");
            message.AppendLine($"Message: {exception.InnerException.Message}");
            message.AppendLine("Stack Trace:");
            message.AppendLine(exception.InnerException.StackTrace);
        }

        Log(message.ToString(), level);
    }

    private void ConsoleLog(LogEntry entry)
    {
        var formattedMessage = FormatLogEntry(entry);
        
        if (ConsoleColorsEnabled)
        {
            var colors = LogLevelColors.ContainsKey(entry.Level) 
                ? LogLevelColors[entry.Level] 
                : (ConsoleColor.White, ConsoleColor.Black);
            
            var originalForeground = Console.ForegroundColor;
            var originalBackground = Console.BackgroundColor;

            try
            {
                Console.ForegroundColor = colors.Item1;
                Console.BackgroundColor = colors.Item2;

                if (entry.Level >= LogLevel.Error)
                {
                    Console.Error.WriteLine(formattedMessage);
                }
                else
                {
                    Console.WriteLine(formattedMessage);
                }
            }
            finally
            {
                Console.ForegroundColor = originalForeground;
                Console.BackgroundColor = originalBackground;
            }
        }
        else
        {
            if (entry.Level >= LogLevel.Error)
            {
                Console.Error.WriteLine(formattedMessage);
            }
            else
            {
                Console.WriteLine(formattedMessage);
            }
        }
    }

    private async Task ProcessLogQueue()
    {
        var batch = new List<LogEntry>();
        var lastWriteTime = DateTime.Now;

        while (!_cancellationTokenSource.Token.IsCancellationRequested)
        {
            try
            {
                // Try to dequeue all available entries
                while (_logQueue.TryDequeue(out var entry))
                {
                    batch.Add(entry);

                    // Write if we've reached the batch size
                    if (batch.Count >= MaxBatchSize)
                    {
                        await WriteBatchToFileAsync(batch);
                        batch.Clear();
                        lastWriteTime = DateTime.Now;
                    }
                }

                // Write if we've waited long enough
                if (batch.Count > 0 && (DateTime.Now - lastWriteTime).TotalMilliseconds >= MaxBatchWaitTime)
                {
                    await WriteBatchToFileAsync(batch);
                    batch.Clear();
                    lastWriteTime = DateTime.Now;
                }

                // If we have no entries, wait a bit before checking again
                if (batch.Count == 0)
                {
                    await Task.Delay(100, _cancellationTokenSource.Token);
                }
            }
            catch (OperationCanceledException)
            {
                break;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error processing log queue: {ex.Message}");
            }
        }

        // Write any remaining entries
        if (batch.Count > 0)
        {
            await WriteBatchToFileAsync(batch);
        }
    }

    private async Task WriteBatchToFileAsync(List<LogEntry> batch)
    {
        if (batch.Count == 0) return;

        try
        {
            var today = DateTime.Now.Date;
            if (_currentLogFile == null || _currentLogDate != today)
            {
                _currentLogDate = today;
                _currentLogFile = GetNextLogFilePath();
            }

            var sb = new StringBuilder();
            foreach (var entry in batch)
            {
                sb.AppendLine(FormatLogEntry(entry));
            }

            using var semaphore = new SemaphoreSlim(1, 1);
            await semaphore.WaitAsync();

            try
            {
                var fileInfo = new FileInfo(_currentLogFile);
                if (fileInfo.Exists && fileInfo.Length > MaxFileSizeMB * 1024 * 1024)
                {
                    _currentLogFile = GetNextLogFilePath();
                }

                using var fileStream = new FileStream(_currentLogFile, FileMode.Append, FileAccess.Write, FileShare.Read);
                using var streamWriter = new StreamWriter(fileStream, Encoding.UTF8, 65536); // 64KB buffer
                await streamWriter.WriteAsync(sb.ToString());
            }
            finally
            {
                semaphore.Release();
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error writing to log file: {ex.Message}");
        }
    }

    private string GetNextLogFilePath()
    {
        var basePath = Path.Combine(_logDirectory, $"{_currentLogDate:yyyy-MM-dd}");
        var index = 0;
        string filePath;

        do
        {
            filePath = index == 0 ? $"{basePath}.log" : $"{basePath}_{index}.log";
            index++;
        } while (File.Exists(filePath) && index < MaxFilesPerDay);

        if (index >= MaxFilesPerDay)
        {
            // If we've reached the maximum number of files, delete the oldest one
            var oldestFile = Directory.GetFiles(_logDirectory, $"{_currentLogDate:yyyy-MM-dd}*.log")
                .OrderBy(f => File.GetCreationTime(f))
                .FirstOrDefault();

            if (oldestFile != null)
            {
                try
                {
                    File.Delete(oldestFile);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error deleting old log file: {ex.Message}");
                }
            }
        }

        return filePath;
    }

    private string FormatLogEntry(LogEntry entry)
    {
        return $"{entry.Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{entry.Level}] {entry.LoggerName}: {entry.Message}";
    }

    /// <summary>
    /// Cleans up old log files.
    /// </summary>
    public void CleanupOldLogs()
    {
        if (!Directory.Exists(_logDirectory)) return;

        var cutoffDate = DateTime.Now.AddDays(-MaxLogFileAge);
        var logFiles = Directory.GetFiles(_logDirectory, "*.log");

        foreach (var file in logFiles)
        {
            try
            {
                var fileInfo = new FileInfo(file);
                if (fileInfo.CreationTime < cutoffDate)
                {
                    fileInfo.Delete();
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error cleaning up log file {file}: {ex.Message}");
            }
        }
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        if (_isDisposed) return;

        _cancellationTokenSource.Cancel();
        _processQueueTask?.Wait(TimeSpan.FromSeconds(5));
        _cancellationTokenSource.Dispose();

        var remainingEntries = new List<LogEntry>();
        while (_logQueue.TryDequeue(out var entry))
        {
            remainingEntries.Add(entry);
        }
        if (remainingEntries.Count > 0)
        {
            WriteBatchToFileAsync(remainingEntries).Wait();
        }

        _isDisposed = true;
        GC.SuppressFinalize(this);
    }

    private class LogEntry
    {
        public DateTime Timestamp { get; set; }
        public LogLevel Level { get; set; }
        public string? Message { get; set; }
        public string LoggerName { get; set; } = string.Empty;
    }
}
