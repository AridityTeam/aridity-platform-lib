using System;
using System.Text;

namespace AridityTeam.Text;

/// <summary>
/// 
/// </summary>
public class UtlString : IEquatable<UtlString>
{
    private string _value = string.Empty;
    private int _allocSize;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="initialAlloc"></param>
    public UtlString(int initialAlloc = 16)
    {
        _allocSize = initialAlloc;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="newSize"></param>
    public void Reserve(int newSize)
    {
        if (newSize > _allocSize)
        {
            var sb = new StringBuilder(_value, newSize);
            _allocSize = newSize;
            _value = sb.ToString();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="text"></param>
    public void Append(string text)
    {
        var newLength = _value.Length + text.Length;
        if (newLength > _allocSize)
        {
            Reserve(Math.Max(newLength, _allocSize * 2));
        }
        _value += text;
    }

    /// <inheritdoc/>
    public override string ToString() => _value;

    /// <inheritdoc/>
    public bool Equals(UtlString? other) => _value == other?._value;

    /// <summary>
    /// Determines whether the current string contains the specified substring.
    /// </summary>
    /// <param name="substring">The substring to locate within the current string. Cannot be <see langword="null"/>.</param>
    /// <returns><see langword="true"/> if the specified substring occurs within the current string; otherwise, <see
    /// langword="false"/>.</returns>
    public bool Contains(string? substring) => substring != null && _value.Contains(substring);

    /// <summary>
    /// Implicitly converts a <see cref="UtlString"/> instance to its underlying string value.
    /// </summary>
    /// <param name="utlString">The <see cref="UtlString"/> instance to convert.</param>
    public static implicit operator string(UtlString utlString) => utlString._value;

    /// <summary>
    /// Implicitly converts a <see cref="UtlString"/> instance to its underlying string value.
    /// </summary>
    /// <param name="v">The string the value to set.</param>
    public static implicit operator UtlString(string v) => new UtlString(v.Length) { _value = v };
}
