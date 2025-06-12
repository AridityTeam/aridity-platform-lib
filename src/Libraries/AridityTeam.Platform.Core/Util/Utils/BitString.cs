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

namespace AridityTeam.Util.Utils;

/// <summary>
/// 
/// </summary>
public class BitString
{
    private uint[] _bits;
    private const int BitsPerElement = 32;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="initialBits"></param>
    public BitString(int initialBits = 32)
    {
        _bits = new uint[(initialBits + BitsPerElement - 1) / BitsPerElement];
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="bitIndex"></param>
    /// <returns></returns>
    public bool Get(int bitIndex)
    {
        var element = bitIndex / BitsPerElement;
        var bit = bitIndex % BitsPerElement;
        return (_bits[element] & (1u << bit)) != 0;
    }

    /// <summary>
    /// Sets the value of the specified bit in the collection.
    /// </summary>
    /// <remarks>This method modifies the state of the bit at the specified index. Ensure that <paramref
    /// name="bitIndex"/> is within the valid range of the bit collection to avoid an exception.</remarks>
    /// <param name="bitIndex">The zero-based index of the bit to set. Must be within the valid range of bits.</param>
    /// <param name="value"><see langword="true"/> to set the bit to 1; <see langword="false"/> to set the bit to 0.</param>
    public void Set(int bitIndex, bool value)
    {
        var element = bitIndex / BitsPerElement;
        var bit = bitIndex % BitsPerElement;
        if (value)
            _bits[element] |= 1u << bit;
        else
            _bits[element] &= ~(1u << bit);
    }
}
