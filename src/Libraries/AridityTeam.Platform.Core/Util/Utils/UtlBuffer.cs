using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AridityTeam.Util.Utils;

/// <summary>
/// 
/// </summary>
internal class UtlBuffer : IDisposable
{
    private byte[] _buffer;
    private int _size;
    private int _position;
    private bool _isTextMode;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="initialSize"></param>
    /// <param name="isTextMode"></param>
    public UtlBuffer(int initialSize = 256, bool isTextMode = false)
    {
        _buffer = new byte[initialSize];
        _isTextMode = isTextMode;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="data"></param>
    public void Put(byte[] data)
    {
        var requiredSize = _position + data.Length;
        if (requiredSize > _buffer.Length)
        {
            Array.Resize(ref _buffer, Math.Max(requiredSize, _buffer.Length * 2));
        }
        Array.Copy(data, 0, _buffer, _position, data.Length);
        _position += data.Length;
        _size = Math.Max(_size, _position);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public byte[] GetData() => _buffer.Take(_size).ToArray();

    /// <inheritdoc/>
    public void Dispose()
    {
        Array.Clear(_buffer, 0, _buffer.Length);
    }
}
