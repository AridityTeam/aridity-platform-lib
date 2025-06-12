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
using System.Collections.Generic;

namespace AridityTeam.Util;

/// <summary>
/// An observable <see cref="List{T}"/>. <para/>
/// Warning: Use <see cref="Concurrency.ObservableConcurrentBag{T}"/> if you want to have thread-safe lists
/// </summary>
/// <typeparam name="T"></typeparam>
[Serializable]
public class ObservableList<T> : List<T?>
{
    /// <summary>
    /// An action to be invoked whenever a new item is added to the collection.
    /// </summary>
    public event Action<T?>? ItemAdded;

    /// <summary>
    /// An action to be invoked whenever an item is removed from the collection.
    /// </summary>
    public event Action<T?>? ItemRemoved;

    /// <summary>
    /// Initializes a new <seealso cref="ObservableList{T}"/>.
    /// </summary>
    public ObservableList()
    { }

    /// <summary>
    /// Initializes a new <seealso cref="ObservableList{T}"/>.
    /// </summary>
    /// <param name="capacity"/>
    public ObservableList(int capacity) : base(capacity) { }

    /// <summary>
    /// Initializes a new <seealso cref="ObservableList{T}"/>.
    /// </summary>
    /// <param name="collection"/>
    public ObservableList(IEnumerable<T> collection) : base(collection) { }

    /// <summary>
    /// Adds an object to the list.
    /// </summary>
    /// <param name="item"></param>
    public new void Add(T? item)
    {
        base.Add(item);
        ItemAdded?.Invoke(item);
    }

    /// <summary>
    /// Removes an object from the list.
    /// </summary>
    /// <param name="item"></param>
    public new bool Remove(T? item)
    {
        if (base.Remove(item))
        {
            ItemRemoved?.Invoke(item);
            return true;
        }
        return false;
    }
}
