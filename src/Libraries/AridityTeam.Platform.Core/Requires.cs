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
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace AridityTeam;

/// <summary>
/// Common runtime checks that throw appropriate exceptions for validation failures.
/// </summary>
public static class Requires
{
    /// <summary>
    /// Throws an exception if the specified parameter's value is null.
    /// </summary>
    /// <param name="value">The value of the argument.</param>
    /// <param name="parameterName">The name of the specified parameter to include in any thrown exception.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <see langword="null"/>.</exception>
    [DebuggerStepThrough]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void NotNull<T>([ValidatedNotNull, NotNull] T? value, [CallerArgumentExpression(nameof(value))] string? parameterName = null)
        where T : class
    {
        if (value is null)
            throw new ArgumentNullException(parameterName);
    }

    /// <summary>
    /// Throws an exception if the specified parameter's value is null.
    /// </summary>
    /// <param name="value">The value of the argument.</param>
    /// <param name="parameterName">The name of the specified parameter to include in any thrown exception.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <see langword="null"/>.</exception>
    [DebuggerStepThrough]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void NotNull<T>([ValidatedNotNull, NotNull] T? value, [CallerArgumentExpression(nameof(value))] string? parameterName = null)
        where T : struct
    {
        if (!value.HasValue)
            throw new ArgumentNullException(parameterName);
    }

    /// <summary>
    /// Throws an exception if the specified parameter's value is null or empty.
    /// </summary>
    /// <param name="value">The value of the argument.</param>
    /// <param name="parameterName">The name of the specified parameter to include in any thrown exception.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">Thrown if <paramref name="value"/> is empty.</exception>
    [DebuggerStepThrough]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void NotNullOrEmpty([ValidatedNotNull, NotNull] string? value, [CallerArgumentExpression(nameof(value))] string? parameterName = null)
    {
        if (value is null)
            throw new ArgumentNullException(parameterName);

        if (value.Length == 0)
            throw new ArgumentException("Value cannot be empty.", parameterName);
    }

    /// <summary>
    /// Throws an exception if the specified parameter's value is null, empty, or consists only of white-space characters.
    /// </summary>
    /// <param name="value">The value of the argument.</param>
    /// <param name="parameterName">The name of the specified parameter to include in any thrown exception.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">Thrown if <paramref name="value"/> is empty or consists only of white-space characters.</exception>
    [DebuggerStepThrough]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void NotNullOrWhiteSpace([ValidatedNotNull, NotNull] string? value, [CallerArgumentExpression(nameof(value))] string? parameterName = null)
    {
        if (value is null)
            throw new ArgumentNullException(parameterName);

        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Value cannot be empty or consist only of white-space characters.", parameterName);
    }

    /// <summary>
    /// Throws an exception if the specified parameters are not equal to each other.
    /// </summary>
    /// <param name="actual">The value of the actual argument.</param>
    /// <param name="expected">The value of the expected argument.</param>
    /// <param name="parameterName">The name of the parameter to include in any thrown exception.</param>
    /// <exception cref="ArgumentException">Thrown if <paramref name="actual"/> is not equal to <paramref name="expected"/>.</exception>
    [DebuggerStepThrough]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void EqualTo<T>(T actual, T expected, [CallerArgumentExpression(nameof(actual))] string? parameterName = null)
    {
        if (!EqualityComparer<T>.Default.Equals(actual, expected))
            throw new ArgumentException($"Expected value: {expected}, Actual value: {actual}", parameterName);
    }

    /// <summary>
    /// Throws an exception if the condition is false.
    /// </summary>
    /// <param name="condition">The condition to check.</param>
    /// <param name="message">The message to include in the exception.</param>
    /// <exception cref="ArgumentException">Thrown if <paramref name="condition"/> is false.</exception>
    [DebuggerStepThrough]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void True(bool condition, string? message = null)
    {
        if (!condition)
            throw new ArgumentException(message ?? "Condition must be true.");
    }

    /// <summary>
    /// Throws an exception if the condition is true.
    /// </summary>
    /// <param name="condition">The condition to check.</param>
    /// <param name="message">The message to include in the exception.</param>
    /// <exception cref="ArgumentException">Thrown if <paramref name="condition"/> is true.</exception>
    [DebuggerStepThrough]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void False(bool condition, string? message = null)
    {
        if (condition)
            throw new ArgumentException(message ?? "Condition must be false.");
    }

    /// <summary>
    /// Throws an exception if the specified value is not in the specified range.
    /// </summary>
    /// <typeparam name="T">The type of the value to check.</typeparam>
    /// <param name="value">The value to check.</param>
    /// <param name="min">The minimum value.</param>
    /// <param name="max">The maximum value.</param>
    /// <param name="parameterName">The name of the parameter to include in any thrown exception.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="value"/> is not in the specified range.</exception>
    [DebuggerStepThrough]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InRange<T>(T value, T min, T max, [CallerArgumentExpression(nameof(value))] string? parameterName = null)
        where T : IComparable<T>
    {
        if (value.CompareTo(min) < 0 || value.CompareTo(max) > 0)
            throw new ArgumentOutOfRangeException(parameterName, value, $"Value must be between {min} and {max}.");
    }

    /// <summary>
    /// Throws an exception if the collection is null or empty.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    /// <param name="collection">The collection to check.</param>
    /// <param name="parameterName">The name of the parameter to include in any thrown exception.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="collection"/> is null.</exception>
    /// <exception cref="ArgumentException">Thrown if <paramref name="collection"/> is empty.</exception>
    [DebuggerStepThrough]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void NotNullOrEmpty<T>([ValidatedNotNull, NotNull] IEnumerable<T>? collection, [CallerArgumentExpression(nameof(collection))] string? parameterName = null)
    {
        if (collection is null)
            throw new ArgumentNullException(parameterName);

        if (!collection.Any())
            throw new ArgumentException("Collection must not be empty.", parameterName);
    }

    /// <summary>
    /// Throws an exception if the collection is empty.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    /// <param name="collection">The collection to check.</param>
    /// <param name="parameterName">The name of the parameter to include in any thrown exception.</param>
    /// <exception cref="ArgumentException">Thrown if <paramref name="collection"/> is empty.</exception>
    [DebuggerStepThrough]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void NotEmpty<T>(IEnumerable<T> collection, [CallerArgumentExpression(nameof(collection))] string? parameterName = null)
    {
        if (!collection.Any())
            throw new ArgumentException("Collection must not be empty.", parameterName);
    }

    /// <summary>
    /// Throws an exception if the object's state is invalid.
    /// </summary>
    /// <param name="condition">The condition that must be true for the state to be valid.</param>
    /// <param name="message">The message to include in the exception.</param>
    /// <exception cref="InvalidOperationException">Thrown if <paramref name="condition"/> is false.</exception>
    [DebuggerStepThrough]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ValidState(bool condition, string? message = null)
    {
        if (!condition)
            throw new InvalidOperationException(message ?? "Object is in an invalid state.");
    }

    /// <summary>
    /// Throws an exception if the operation is invalid.
    /// </summary>
    /// <param name="condition">The condition that must be true for the operation to be valid.</param>
    /// <param name="message">The message to include in the exception.</param>
    /// <exception cref="InvalidOperationException">Thrown if <paramref name="condition"/> is false.</exception>
    [DebuggerStepThrough]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ValidOperation(bool condition, string? message = null)
    {
        if (!condition)
            throw new InvalidOperationException(message ?? "Operation is not valid in the current state.");
    }

    /// <summary>
    /// Throws an exception if the string does not match the specified pattern.
    /// </summary>
    /// <param name="value">The string to validate.</param>
    /// <param name="pattern">The regular expression pattern to match.</param>
    /// <param name="parameterName">The name of the parameter to include in any thrown exception.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> or <paramref name="pattern"/> is null.</exception>
    /// <exception cref="ArgumentException">Thrown if <paramref name="value"/> does not match the pattern.</exception>
    [DebuggerStepThrough]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void MatchesPattern([ValidatedNotNull, NotNull] string? value, [ValidatedNotNull, NotNull] string? pattern, [CallerArgumentExpression(nameof(value))] string? parameterName = null)
    {
        NotNull(value, parameterName);
        NotNull(pattern, nameof(pattern));

        if (!Regex.IsMatch(value, pattern))
            throw new ArgumentException($"Value does not match the required pattern: {pattern}", parameterName);
    }

    /// <summary>
    /// Throws an exception if the string is not a valid email address.
    /// </summary>
    /// <param name="value">The email address to validate.</param>
    /// <param name="parameterName">The name of the parameter to include in any thrown exception.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is null.</exception>
    /// <exception cref="ArgumentException">Thrown if <paramref name="value"/> is not a valid email address.</exception>
    [DebuggerStepThrough]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ValidEmail([ValidatedNotNull, NotNull] string? value, [CallerArgumentExpression(nameof(value))] string? parameterName = null)
    {
        NotNull(value, parameterName);
        const string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        
        if (!Regex.IsMatch(value, emailPattern))
            throw new ArgumentException("Value is not a valid email address.", parameterName);
    }

    /// <summary>
    /// Throws an exception if the string is not a valid URL.
    /// </summary>
    /// <param name="value">The URL to validate.</param>
    /// <param name="parameterName">The name of the parameter to include in any thrown exception.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is null.</exception>
    /// <exception cref="ArgumentException">Thrown if <paramref name="value"/> is not a valid URL.</exception>
    [DebuggerStepThrough]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ValidUrl([ValidatedNotNull, NotNull] string? value, [CallerArgumentExpression(nameof(value))] string? parameterName = null)
    {
        NotNull(value, parameterName);
        if (!Uri.TryCreate(value, UriKind.Absolute, out _))
            throw new ArgumentException("Value is not a valid URL.", parameterName);
    }

    /// <summary>
    /// Throws an exception if the string length is not within the specified range.
    /// </summary>
    /// <param name="value">The string to validate.</param>
    /// <param name="minLength">The minimum allowed length.</param>
    /// <param name="maxLength">The maximum allowed length.</param>
    /// <param name="parameterName">The name of the parameter to include in any thrown exception.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="value"/> length is not within the specified range.</exception>
    [DebuggerStepThrough]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void LengthInRange([ValidatedNotNull, NotNull] string? value, int minLength, int maxLength, [CallerArgumentExpression(nameof(value))] string? parameterName = null)
    {
        NotNull(value, parameterName);
        if (value.Length < minLength || value.Length > maxLength)
            throw new ArgumentOutOfRangeException(parameterName, value.Length, $"String length must be between {minLength} and {maxLength} characters.");
    }

    /// <summary>
    /// Throws an exception if the collection contains any null elements.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    /// <param name="collection">The collection to validate.</param>
    /// <param name="parameterName">The name of the parameter to include in any thrown exception.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="collection"/> is null.</exception>
    /// <exception cref="ArgumentException">Thrown if <paramref name="collection"/> contains any null elements.</exception>
    [DebuggerStepThrough]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void NoNullElements<T>([ValidatedNotNull, NotNull] IEnumerable<T?>? collection, [CallerArgumentExpression(nameof(collection))] string? parameterName = null)
        where T : class
    {
        NotNull(collection, parameterName);
        if (collection.Any(item => item is null))
            throw new ArgumentException(ExceptionStrings.Validation_HasNullElements, parameterName);
    }

    /// <summary>
    /// Throws an exception if the collection contains any duplicate elements.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    /// <param name="collection">The collection to validate.</param>
    /// <param name="parameterName">The name of the parameter to include in any thrown exception.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="collection"/> is null.</exception>
    /// <exception cref="ArgumentException">Thrown if <paramref name="collection"/> contains any duplicate elements.</exception>
    [DebuggerStepThrough]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void NoDuplicates<T>([ValidatedNotNull, NotNull] IEnumerable<T>? collection, [CallerArgumentExpression(nameof(collection))] string? parameterName = null)
    {
        NotNull(collection, parameterName);
        if (collection.Distinct().Count() != collection.Count())
            throw new ArgumentException(ExceptionStrings.Validation_HasCollectionDuplicates, parameterName);
    }

    /// <summary>
    /// Throws an exception if the value is not one of the specified valid values.
    /// </summary>
    /// <typeparam name="T">The type of the value to validate.</typeparam>
    /// <param name="value">The value to validate.</param>
    /// <param name="validValues">The set of valid values.</param>
    /// <param name="parameterName">The name of the parameter to include in any thrown exception.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="validValues"/> is null.</exception>
    /// <exception cref="ArgumentException">Thrown if <paramref name="value"/> is not one of the valid values.</exception>
    [DebuggerStepThrough]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void OneOf<T>(T value, [ValidatedNotNull, NotNull] IEnumerable<T>? validValues, [CallerArgumentExpression(nameof(value))] string? parameterName = null)
    {
        NotNull(validValues, nameof(validValues));
        if (!validValues.Contains(value))
            throw new ArgumentException(string.Format(ExceptionStrings.Validation_NotOneOf, string.Join(", ", validValues)), parameterName);
    }

    /// <summary>
    /// Throws an exception if the value is not a valid enum value.
    /// </summary>
    /// <typeparam name="T">The enum type.</typeparam>
    /// <param name="value">The value to validate.</param>
    /// <param name="parameterName">The name of the parameter to include in any thrown exception.</param>
    /// <exception cref="ArgumentException">Thrown if <paramref name="value"/> is not a valid enum value.</exception>
    [DebuggerStepThrough]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ValidEnum<T>(T value, [CallerArgumentExpression(nameof(value))] string? parameterName = null)
        where T : struct, Enum
    {
        if (!Enum.IsDefined(typeof(T), value))
            throw new ArgumentException(string.Format(ExceptionStrings.Validation_InvalidEnum, typeof(T).Name), parameterName);
    }

    /// <summary>
    /// Throws an exception if the value is not a valid file path.
    /// </summary>
    /// <param name="value">The path to validate.</param>
    /// <param name="parameterName">The name of the parameter to include in any thrown exception.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is null.</exception>
    /// <exception cref="ArgumentException">Thrown if <paramref name="value"/> is not a valid file path.</exception>
    [DebuggerStepThrough]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ValidFilePath([ValidatedNotNull, NotNull] string? value, [CallerArgumentExpression(nameof(value))] string? parameterName = null)
    {
        NotNull(value, parameterName);
        try
        {
            var path = Path.GetFullPath(value);
            if (string.IsNullOrEmpty(path))
                throw new ArgumentException(ExceptionStrings.Validation_InvalidFile, parameterName);
        }
        catch (Exception)
        {
            throw new ArgumentException(ExceptionStrings.Validation_InvalidFile, parameterName);
        }
    }

    /// <summary>
    /// Throws an exception if the value is not a valid directory path.
    /// </summary>
    /// <param name="value">The path to validate.</param>
    /// <param name="parameterName">The name of the parameter to include in any thrown exception.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is null.</exception>
    /// <exception cref="ArgumentException">Thrown if <paramref name="value"/> is not a valid directory path.</exception>
    [DebuggerStepThrough]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ValidDirectoryPath([ValidatedNotNull, NotNull] string? value, [CallerArgumentExpression(nameof(value))] string? parameterName = null)
    {
        NotNull(value, parameterName);
        try
        {
            var path = Path.GetFullPath(value);
            if (string.IsNullOrEmpty(path) || Path.HasExtension(path))
                throw new ArgumentException(ExceptionStrings.Validation_InvalidDirectory, parameterName);
        }
        catch (Exception)
        {
            throw new ArgumentException(ExceptionStrings.Validation_InvalidDirectory, parameterName);
        }
    }

    /// <summary>
    /// Throws an exception if the value is not a valid IP address.
    /// </summary>
    /// <param name="value">The IP address to validate.</param>
    /// <param name="parameterName">The name of the parameter to include in any thrown exception.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is null.</exception>
    /// <exception cref="ArgumentException">Thrown if <paramref name="value"/> is not a valid IP address.</exception>
    [DebuggerStepThrough]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ValidIpAddress([ValidatedNotNull, NotNull] string? value, [CallerArgumentExpression(nameof(value))] string? parameterName = null)
    {
        NotNull(value, parameterName);
        if (!System.Net.IPAddress.TryParse(value, out _))
            throw new ArgumentException(ExceptionStrings.Validation_InvalidIP, parameterName);
    }

    /// <summary>
    /// Throws an exception if the value is not a valid port number.
    /// </summary>
    /// <param name="value">The port number to validate.</param>
    /// <param name="parameterName">The name of the parameter to include in any thrown exception.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="value"/> is not a valid port number.</exception>
    [DebuggerStepThrough]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ValidPort(int value, [CallerArgumentExpression(nameof(value))] string? parameterName = null)
    {
        if (value < 0 || value > 65535)
            throw new ArgumentOutOfRangeException(parameterName, value, ExceptionStrings.Validation_PortInvalid);
    }
}
