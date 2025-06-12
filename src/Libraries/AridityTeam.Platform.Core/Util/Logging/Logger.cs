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

namespace AridityTeam.Util.Logging;

/// <summary>
/// Ah yes, logging.
/// </summary>
public class Logger(bool enableConsoleLogging = true, bool enableFileLogging = false) : ILogger
{
    /// <summary>
    /// 
    /// </summary>
    public bool ConsoleLoggingEnabled
    {
        get => _isConsoleLoggingEnabled;
        set => _isConsoleLoggingEnabled = value;
    }
    private bool _isConsoleLoggingEnabled = enableConsoleLogging;

    /// <summary>
    /// 
    /// </summary>
    public bool FileLoggingEnabled
    {
        get => _isFileLoggingEnabled;
        set => _isFileLoggingEnabled = value;
    }
    private bool _isFileLoggingEnabled = enableFileLogging;

    private string FormatMessage(string? message, LogLevel level = LogLevel.Info)
    {
        return $"{level.ToFriendlyString().ToUpper()} - {DateTime.Now:HH-mm-ss-MM-dd-yyyy}: {message}";
    }

    /// <summary>
    /// Logs a message to the specified enabled logger.
    /// </summary>
    /// <param name="message">A message to write.</param>
    /// <param name="level">Logging level.</param>
    public void Log(string? message, LogLevel level = LogLevel.Info)
    {
        if (_isConsoleLoggingEnabled)
            ConsoleLog(message, level);

        if (_isFileLoggingEnabled)
            FileLog($"{AppDomain.CurrentDomain.BaseDirectory}/{DateTime.Now:HH-mm-ss-MM-dd-yyyy}.log", message, level);
    }

    private void FileLog(string filePath, string? message, LogLevel level = LogLevel.Info)
    {
        using var sw = new StreamWriter(filePath)
        {
            AutoFlush = true,
        };
        sw.WriteLine(FormatMessage(message, level));
    }

    private void ConsoleLog(string? message, LogLevel level = LogLevel.Info) =>
        Console.WriteLine(FormatMessage(message, level));

    /// <inheritdoc/>
    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}
