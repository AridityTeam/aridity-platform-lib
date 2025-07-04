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
using Microsoft.Win32;
using System.Runtime.Versioning;

namespace AridityTeam.Util.DRM;

/// <summary>
/// Only contains the strings, booleans and functions used to run a Steam game,
/// gets a specific Steam folder's directory, and check if the game is running.
/// Only supports Windows.
/// </summary>
[SupportedOSPlatform("windows")]
public class Steam
{
    /// <summary>
    /// Checks if the Steam client is running.
    /// </summary>
    /// <returns><see langword="true"/> if "steam.exe" or "steamwebhelper.exe" instances are running.</returns>
    public bool IsSteamRunning() => 
        Process.GetProcessesByName("Steam").Length != 0 || // start getting the process using the window title
        Process.GetProcessesByName("steam.exe").Length != 0 ||
        Process.GetProcessesByName("steamwebhelper.exe").Length != 0;

    /// <summary>
    /// Gets the path to the "sourcemods" folder in the Steam directory.
    /// </summary>
    public string SteamExePath
    {
        get
        {
            var steamExe = RegistryUtil.GetString(
                Registry.CurrentUser,
                @"Software\Valve\Steam\SteamExe"
            );

            Requires.NotNullOrWhiteSpace(steamExe);

            return steamExe;
        }
    }

    /// <summary>
    /// Launches an Steam application by its app ID.
    /// </summary>
    /// <param name="appId">The value of the app ID.</param>
    /// <returns>
    ///     <see langword="true"/> on successful launch.
    ///     (well actually it returns true, when it successfully launches Steam :/)
    /// </returns>
    public bool RunByID(long appId)
    {
        Requires.NotNull<long>(appId);
        using var p = new Process();
        p.StartInfo.FileName = SteamExePath;
        p.StartInfo.Arguments = $"-applaunch {appId}";
        p.StartInfo.UseShellExecute = false;
        p.StartInfo.CreateNoWindow = true;
        return p.Start();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="appId"></param>
    /// <returns></returns>
    public bool IsGameRunningByID(long appId)
    {
        throw new NotImplementedException("This method is not implemented yet. " +
            "You can use the Process.GetProcessesByName method to check if the game is running by its name.");
    }
}
