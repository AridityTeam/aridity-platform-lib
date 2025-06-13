using System;
using System.Text.RegularExpressions;

namespace AridityTeam.Util.Git;

/// <summary>
/// 
/// </summary>
internal static class GitProgressParser
{
    // Matches patterns like: "Receiving objects:  67% (354/527)"
    private static readonly Regex ProgressRegex = new(
        @"([\w\s]+):\s+(\d+)%\s*(?:\((\d+)/(\d+)\))?",
        RegexOptions.Compiled);

    private static readonly string[] CloneStages = {
        "Receiving objects",
        "Resolving deltas",
        "Updating files"
    };

    /// <summary>
    /// 
    /// </summary>
    /// <param name="line"></param>
    /// <returns></returns>
    public static (string Operation, int Percentage)? ParseProgress(string line)
    {
        var match = ProgressRegex.Match(line);
        if (!match.Success) return null;

        var operation = match.Groups[1].Value.Trim();
        var percentage = int.Parse(match.Groups[2].Value);

        return (operation, percentage);
    }
}