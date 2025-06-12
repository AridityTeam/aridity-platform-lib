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

namespace AridityTeam.Util.Utils;

/// <summary>
/// Represents a result that can either be successful with a value or failed with an error message.
/// </summary>
/// <typeparam name="T">The type of the value in case of success.</typeparam>
public class Result<T>
{
    /// <summary>
    /// Gets a value indicating whether the result is successful.
    /// </summary>
    public bool IsSuccess { get; }

    /// <summary>
    /// Gets the value of the result. This will be default(T) if the result is a failure.
    /// </summary>
    public T? Value { get; }

    /// <summary>
    /// Gets the error message. This will be null if the result is successful.
    /// </summary>
    public string? Error { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Result{T}"/> class with a successful result.
    /// </summary>
    /// <param name="value">The value of the successful result.</param>
    private Result(T value)
    {
        IsSuccess = true;
        Value = value;
        Error = null;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Result{T}"/> class with a failure result.
    /// </summary>
    /// <param name="error">The error message describing the failure.</param>
    private Result(string error)
    {
        IsSuccess = false;
        Value = default;
        Error = error;
    }

    /// <summary>
    /// Creates a successful result with the specified value.
    /// </summary>
    /// <param name="value">The value to be contained in the successful result.</param>
    /// <returns>A new <see cref="Result{T}"/> instance containing the successful value.</returns>
    public static Result<T> Success(T value) => new(value);

    /// <summary>
    /// Creates a failure result with the specified error message.
    /// </summary>
    /// <param name="error">The error message describing the failure.</param>
    /// <returns>A new <see cref="Result{T}"/> instance containing the error message.</returns>
    public static Result<T> Failure(string error) => new(error);
}
