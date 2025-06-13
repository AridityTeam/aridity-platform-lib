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
using System.Diagnostics.CodeAnalysis;
using AridityTeam.Util.Logging;
using AridityTeam.Util.Utils;

namespace AridityTeam;

/// <summary>
/// This static class exposes the interface implemented internally in an shared library into an object specified that can be appended to a
/// global variable that can be accessed by other libraries.
/// 
/// In other cases, you can use this utility to call a plugin's function.
/// </summary>
public static class InterfaceExposeUtil
{
    /// <summary>
    /// Exposes a single interface implementation as an object after version validation.
    /// </summary>
    /// <typeparam name="TClass">The concrete class type that implements the interface</typeparam>
    /// <typeparam name="TInterface">The interface type to expose</typeparam>
    /// <param name="versionName">The version name to validate against</param>
    /// <returns>The interface implementation as an object</returns>
    /// <exception cref="NullReferenceException">Thrown when version name is <see langword="null"/> or empty</exception>
    /// <exception cref="InvalidOperationException">Thrown when interface implementation is invalid</exception>
    public static TInterface ExposeSingleInterface<TClass, TInterface>(string versionName)
        where TClass : class, new()
        where TInterface : class, IBaseInterface
    {
        Requires.NotNullOrWhiteSpace(versionName);

        var instance = new TClass();
        if (instance is not TInterface @interface)
            throw new InvalidOperationException($"Type '{typeof(TClass).Name}' must implement '{typeof(TInterface).Name}'");

        if (!string.Equals(@interface.VersionName, versionName))
            throw new ObjectNotEqualException($"Version mismatch. Expected: {versionName}, Got: {@interface.VersionName}");

        Logger.GetLogger("InterfaceExposeUtil").Log($"Exposed \"{typeof(TInterface).Name}\" with \"{typeof(TClass).Name}\".");

        return @interface;
    }

    /// <summary>
    /// Tries to expose a single interface implementation with version validation.
    /// </summary>
    /// <returns><see langword="true"/> if the interface was successfully exposed, <see langword="false"/> otherwise</returns>
    public static bool TryExposeSingleInterface<TClass, TInterface>(
        string versionName,
        [NotNullWhen(true)] out TInterface? result)
        where TClass : class, new()
        where TInterface : class, IBaseInterface
    {
        try
        {
            result = ExposeSingleInterface<TClass, TInterface>(versionName);
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    /// <summary>
    /// Creates and validates an interface implementation with a Result pattern.
    /// </summary>
    public static Result<TInterface> CreateInterface<TClass, TInterface>(string versionName)
        where TClass : class, new()
        where TInterface : class, IBaseInterface
    {
        try
        {
            var instance = ExposeSingleInterface<TClass, TInterface>(versionName);
            return Result<TInterface>.Success(instance);
        }
        catch (Exception ex)
        {
            return Result<TInterface>.Failure(ex.Message);
        }
    }
}
