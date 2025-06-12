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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AridityTeam.Util;

/// <summary>
/// Sends output on multiple <see cref="TextWriter"/>s. <para/>
/// https://stackoverflow.com/questions/18726852/redirecting-console-writeline-to-textbox
/// </summary>
public class MultiTextWriter : TextWriter
{
    private readonly IEnumerable<TextWriter> _writers;

    /// <summary>
    /// Initializes a new <seealso cref="MultiTextWriter"/>.
    /// </summary>
    /// <param name="writers">Initial collection of <seealso cref="System.IO.TextWriter"/>s.</param>
    public MultiTextWriter(IEnumerable<TextWriter> writers)
    {
        _writers = [.. writers];
    }

    /// <summary>
    /// Initializes a new <seealso cref="MultiTextWriter"/>.
    /// </summary>
    /// <param name="writers">Initial collection of <seealso cref="System.IO.TextWriter"/>s.</param>
    public MultiTextWriter(params TextWriter[] writers)
    {
        _writers = [.. writers];
    }

    /// <summary>
    /// Adds a <seealso cref="System.IO.TextWriter"/> to the list.
    /// </summary>
    /// <param name="w"></param>
    public void AddWriter(TextWriter w)
    {
        _ = _writers.Append(w);
    }

    /// <inheritdoc/>
    public override void Write(char value)
    {
        foreach (var writer in _writers)
            writer.Write(value);
    }

    /// <inheritdoc/>
    public override void Write(string? value)
    {
        foreach (var writer in _writers)
            writer.Write(value);
    }

    /// <inheritdoc/>
    public override void WriteLine()
    {
        foreach (var writer in _writers)
            writer.WriteLine();
    }

    /// <inheritdoc/>
    public override void WriteLine(char value)
    {
        foreach (var writer in _writers)
            writer.WriteLine(value);
    }

    /// <inheritdoc/>
    public override void WriteLine(string? value)
    {
        foreach (var writer in _writers)
            writer.WriteLine(value);
    }

    /// <inheritdoc/>
    public override void Flush()
    {
        foreach (var writer in _writers)
            writer.Flush();
    }

    /// <inheritdoc/>
    public override async Task FlushAsync()
    {
        foreach (var writer in _writers)
            await writer.FlushAsync();
    }

    /// <inheritdoc/>
    public new void Dispose()
    {
        foreach (var writer in _writers)
            writer.Dispose();
    }

#if NET8_0_OR_GREATER
    /// <inheritdoc/>
    public new async ValueTask DisposeAsync()
    {
        foreach (var writer in _writers)
            await writer.DisposeAsync();
    }
#endif

    /// <inheritdoc/>
    public override void Close()
    {
        foreach (var writer in _writers)
            writer.Close();
    }

    /// <inheritdoc/>
    public override Encoding Encoding => Encoding.ASCII;
}