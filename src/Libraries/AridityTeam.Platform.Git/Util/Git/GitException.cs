using System;

namespace AridityTeam.Util.Git;

/// <summary>
/// 
/// </summary>
[Serializable]
public class GitException : Exception
{
    /// <summary>
    /// 
    /// </summary>
    public GitException() { }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="message"></param>
    public GitException(string message) : base(message) { }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="message"></param>
    /// <param name="inner"></param>
    public GitException(string message, Exception inner) : base(message, inner) { }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="info"></param>
    /// <param name="context"></param>
    [Obsolete]
    protected GitException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}
