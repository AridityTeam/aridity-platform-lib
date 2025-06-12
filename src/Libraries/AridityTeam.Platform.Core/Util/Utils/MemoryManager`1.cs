using System;

namespace AridityTeam.Util.Utils;

/// <summary>
/// 
/// </summary>
/// <typeparam name="T"></typeparam>
public class MemoryManager<T> : IDisposable
{
    private T[] _memory;
    private int _allocationCount;
    private readonly int _growSize;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="initialSize"></param>
    /// <param name="growSize"></param>
    public MemoryManager(int initialSize = 0, int growSize = 16)
    {
        _memory = new T[initialSize];
        _growSize = growSize;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="i"></param>
    /// <returns></returns>
    public T this[int i]
    {
        get => _memory[i];
        set
        {
            EnsureCapacity(i + 1);
            _memory[i] = value;
            _allocationCount = Math.Max(_allocationCount, i + 1);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="newSize"></param>
    private void EnsureCapacity(int newSize)
    {
        if (newSize > _memory.Length)
        {
            var growSize = Math.Max(_growSize, _memory.Length);
            Array.Resize(ref _memory, Math.Max(newSize, _memory.Length + growSize));
        }
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        if (typeof(T).IsClass)
        {
            Array.Clear(_memory, 0, _memory.Length);
        }
    }
}
