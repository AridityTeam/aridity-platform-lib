using System;
using System.Collections.Concurrent;
using System.Threading;

namespace AridityTeam.Util.Utils;

/// <summary>
/// 
/// </summary>
/// <typeparam name="T"></typeparam>
public class Mempool<T> : IDisposable where T : class
{
    private readonly ConcurrentQueue<T> _pool;
    private readonly Func<T> _factory;
    private readonly int _maxSize;
    private int _count;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="factory"></param>
    /// <param name="maxSize"></param>
    public Mempool(Func<T> factory, int maxSize = 1024)
    {
        _pool = new ConcurrentQueue<T>();
        _factory = factory;
        _maxSize = maxSize;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public T Allocate()
    {
        if (_pool.TryDequeue(out var item))
        {
            Interlocked.Decrement(ref _count);
            return item;
        }
        return _factory();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="item"></param>
    public void Free(T item)
    {
        if (_count >= _maxSize)
            return;

        _pool.Enqueue(item);
        Interlocked.Increment(ref _count);
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        while (_pool.TryDequeue(out var item))
        {
            if (item is IDisposable disposable)
                disposable.Dispose();
        }
    }
}
