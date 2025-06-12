using System;
using System.Text;

namespace AridityTeam.Text;

/// <summary>
/// 
/// </summary>
public class UtlString
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
}
