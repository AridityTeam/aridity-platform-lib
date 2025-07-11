﻿/*
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

using AridityTeam;

#if !NET5_0_OR_GREATER || !NETCOREAPP3_0_OR_GREATER
namespace System.Runtime.CompilerServices
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
    internal sealed class CallerArgumentExpressionAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameterName"></param>
        public CallerArgumentExpressionAttribute(string parameterName)
        {
            ParameterName = parameterName;
        }

        /// <summary>
        /// 
        /// </summary>
        public string ParameterName { get; }
    }
}
#endif

#if !NET8_0_OR_GREATER

namespace System.Runtime.Versioning
{
    /// <summary>
    /// stubbed
    /// </summary>
    [System.AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
    public sealed class SupportedOSPlatformAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="platformName"></param>
        public SupportedOSPlatformAttribute(string platformName)
        {
        }
    }
}

namespace System.Collections.Generic
{
    /// <summary>
    /// 
    /// </summary>
    public static class DictionaryExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dict"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool TryAdd<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue value) where TKey : notnull
        {
            Requires.NotNull(dict);

            if (!dict.ContainsKey(key))
            {
                dict.Add(key, value);
                return true;
            }

            return false;
        }
    }
}
#endif