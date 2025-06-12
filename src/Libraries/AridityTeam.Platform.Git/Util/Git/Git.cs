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
using System.Diagnostics;

namespace AridityTeam.Util.Git;

/// <summary>
/// Provides functionality to interact with the Git command-line tool.
/// </summary>
/// <remarks>
/// This class allows users to execute Git commands by specifying the path to the Git executable. It
/// initializes a process configured to run Git commands with standard output and error redirection.
/// </remarks>
public class Git : IGit
{
    private Process p;

    /// <summary>
    /// Initializes a new <seealso cref="Git"/> instance.
    /// </summary>
    /// <param name="gitFilePath">Git executable.</param>
    public Git(string gitFilePath)
    {
        p = new Process()
        {
            StartInfo = new ProcessStartInfo(gitFilePath)
            {
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
            }
        };
    }

    /// <inheritdoc/>
    public event EventHandler<ProgressChangedEventArgs>? CloneProgressChanged;
    /// <inheritdoc/>
    public event EventHandler<ProgressChangedEventArgs>? CheckoutProgressChanged;
    /// <inheritdoc/>
    public event EventHandler<ProgressChangedEventArgs>? FetchProgressChanged;
    /// <inheritdoc/>
    public event EventHandler<ProgressChangedEventArgs>? PullProgressChanged;
    /// <inheritdoc/>
    public event EventHandler<ProgressChangedEventArgs>? PushProgressChanged;

    /// <inheritdoc/>
    public bool Add(string repoPath, string filePath)
    {
        p.StartInfo.Arguments = $"-C \"{repoPath}\" add \"{filePath}\"";
        if (!p.Start())
            throw new GitException("Could not start \"git.exe\".");

        p.WaitForExit();
        return p.ExitCode == 0;
    }

    /// <inheritdoc/>
    public bool Clone(Uri remoteUri, string path, int depth = 0)
    {
        p.StartInfo.Arguments = $"clone --depth \"{depth}\" \"{remoteUri.AbsoluteUri}\" \"{path}\"";
        if (!p.Start())
            throw new GitException("Could not start \"git.exe\".");

        // TODO -- parse Git output to get it's progress percentage to raise in CloneProgressChanged.
        // (Updating files part will be raised into CheckoutProgressChanged.)
        p.WaitForExit();
        CloneProgressChanged?.Invoke(this, new ProgressChangedEventArgs(100));
        CheckoutProgressChanged?.Invoke(this, new ProgressChangedEventArgs(100));
        return p.ExitCode == 0;
    }

    /// <inheritdoc/>
    public bool Commit(string repoPath, string message)
    {
        p.StartInfo.Arguments = $"-C \"{repoPath}\" commit -m \"{message}\"";
        if (!p.Start())
            throw new GitException("Could not start \"git.exe\".");

        p.WaitForExit();
        return p.ExitCode == 0;
    }

    /// <inheritdoc/>
    public bool Fetch(string repoPath, int depth = 0)
    {
        p.StartInfo.Arguments = $"-C \"{repoPath}\" fetch --depth \"{depth}\"";
        if (!p.Start())
            throw new GitException("Could not start \"git.exe\".");

        p.WaitForExit();
        FetchProgressChanged?.Invoke(this, new ProgressChangedEventArgs(100));
        return p.ExitCode == 0;
    }

    /// <inheritdoc/>
    public bool Pull(string repoPath, string remote = "origin", string branch = "master")
    {
        PullProgressChanged?.Invoke(this, new ProgressChangedEventArgs(100));
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public bool Push(string repoPath, string remote = "origin", string branch = "master")
    {
        PushProgressChanged?.Invoke(this, new ProgressChangedEventArgs(100));
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        p.Dispose();
        GC.SuppressFinalize(this);
    }
}
