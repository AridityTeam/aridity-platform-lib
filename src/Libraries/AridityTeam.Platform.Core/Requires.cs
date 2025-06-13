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
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace AridityTeam;

/// <summary>
/// Common runtime checks that throws either an <seealso cref="System.NullReferenceException"/> or an <seealso cref="AridityTeam.ObjectNotEqualException"/>.
/// </summary>
public static class Requires
{
    /// <summary>
    /// Throws an exception if the specified parameter's value is null.
    /// </summary>
    /// <param name="value">The value of the argument.</param>
    /// <param name="parameterName">The name of the specified parameter to include in any thrown exception.</param>
    /// <exception cref="NullReferenceException">Thrown if <paramref name="value"/> is <see langword="null"/>.</exception>
    [DebuggerStepThrough]
    public static void NotNull([ValidatedNotNull, NotNull] object? value, [CallerArgumentExpression(nameof(value))] string? parameterName = null)
    {
        if (value is null)
            throw new NullReferenceException($"Object {parameterName} is null!");
    }

    /// <summary>
    /// Throws an exception if the specified parameter's value is null or empty.
    /// </summary>
    /// <param name="value">The value of the argument.</param>
    /// <param name="parameterName">The name of the specified parameter to include in any thrown exception.</param>
    /// <exception cref="NullReferenceException">Thrown if <paramref name="value"/> is <see langword="null"/>.</exception>
    [DebuggerStepThrough]
    public static void NotNull([ValidatedNotNull, NotNull] string? value, [CallerArgumentExpression(nameof(value))] string? parameterName = null)
    {
        if (value is null)
            throw new NullReferenceException($"String {parameterName} is null!");

        if (value.Length == 0 || value[0] == '\0')
            throw new ArgumentException($"'{parameterName}' cannot be an empty string (\"\") or start with the null character.");
    }

    /// <summary>
    /// Throws an exception if the specified parameter's value is either a null or empty string (with whitespaces).
    /// </summary>
    /// <param name="value"></param>
    /// <param name="parameterName">The name of the specified parameter to include in any thrown exception.</param>
    /// <exception cref="NullReferenceException">Thrown if <paramref name="value"/> is <see langword="null"/>.</exception>
    [DebuggerStepThrough]
    public static void NotNullOrWhiteSpace([ValidatedNotNull, NotNull] string? value, [CallerArgumentExpression(nameof(value))] string? parameterName = null)
    {
        NotNull(value);

        if (string.IsNullOrWhiteSpace(value))
            throw new NullReferenceException($"The argument ({parameterName}) cannot consist entirely of white space characters.");
    }

    /// <summary>
    /// Throws an exception if the specified parameters are not equal to eachother.
    /// </summary>
    /// <param name="actual">The value of the actual argument.</param>
    /// <param name="expected">The value of the expected argument.</param>
    /// <exception cref="ObjectNotEqualException">Thrown if <paramref name="actual"/> is not equal to <paramref name="expected"/>.</exception>
    [DebuggerStepThrough]
    public static void EqualTo(object? actual, object? expected)
    {
        if (actual != expected)
        {
            throw new ObjectNotEqualException($"Expected \"{expected}\" but got \"{actual}\"");
        }
    }

    /// <summary>
    /// Throws an exception if the specified parameters are not equal to eachother.
    /// </summary>
    /// <param name="actual">The value of the actual argument.</param>
    /// <param name="expected">The value of the expected argument.</param>
    /// <exception cref="ObjectNotEqualException">Thrown if <paramref name="actual"/> is not equal to <paramref name="expected"/>.</exception>
    [DebuggerStepThrough]
    public static void EqualTo(bool actual, bool expected)
    {
        if (actual != expected)
        {
            throw new ObjectNotEqualException($"Expected \"{expected}\" but got \"{actual}\"");
        }
    }

    /// <summary>
    /// Throws an exception if the condition doesn't return true.
    /// </summary>
    /// <param name="condition">The condition to check I guess.</param>
    /// <param name="message">The value of the message.</param>
    [DebuggerStepThrough]
    public static void True(bool condition, string? message = null)
    {
        message ??= $"Expected \"True\" but got \"{condition}\"";

        if (!condition)
        {
            throw new ArgumentException(message);
        }
    }

    /// <summary>
    /// Throws an exception if the condition doesn't return false.
    /// </summary>
    /// <param name="condition">The condition to check I guess.</param>
    /// <param name="message">The value of the message.</param>
    [DebuggerStepThrough]
    public static void False(bool condition, string? message = null)
    {
        message ??= $"Expected \"False\" but got \"{condition}\"";

        if (condition)
        {
            throw new ArgumentException(message);
        }
    }

    /// <summary>
    /// Throws an exception if the specified number value is not in between the minimum and max value.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <param name="parameterName"></param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    [DebuggerStepThrough]
    public static void InRange<T>(T value, T min, T max, [CallerArgumentExpression(nameof(value))] string? parameterName = null)
        where T : IComparable<T>
    {
        if (value.CompareTo(min) < 0 || value.CompareTo(max) > 0)
            throw new ArgumentOutOfRangeException(parameterName, $"Value must be between {min} and {max}");
    }

    /// <summary>
    /// Throws an exception if the collection is empty.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="collection"></param>
    /// <param name="parameterName"></param>
    /// <exception cref="ArgumentException"></exception>
    [DebuggerStepThrough]
    public static void NotEmpty<T>(IEnumerable<T>? collection, [CallerArgumentExpression(nameof(collection))] string? parameterName = null)
    {
        NotNull(collection, parameterName);
        if (!collection.Any())
            throw new ArgumentException("Collection must not be empty", parameterName);
    }
}
