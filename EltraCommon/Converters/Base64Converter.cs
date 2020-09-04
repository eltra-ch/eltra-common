// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this code under the MIT license.

using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

/*
 * .NETSTANDARD 2.0 opposite to .NETSTANDARD 2.1 doesn't contain method TryFromBase64String
 * .NETSTANDARD 2.1 isn't supported by UWP at the moment - ridiculous but true (Xamarin iOS and Android can handle it)
 * to keep the compatibility with .NETSTANDARD 2.0/UWP I used code from dotnet repo licensed under MIT
 */

namespace EltraCommon.Converters
{
    /// <summary>
    /// Base64Converter.
    /// </summary>
    public static class Base64Converter
    {
        #region Const

        private const byte EncodingPad = (byte)'='; // '=', for padding

        private static bool IsSpace(this char c) => c == ' ' || c == '\t' || c == '\r' || c == '\n';

        private static ReadOnlySpan<sbyte> DecodingMap => new sbyte[] // rely on C# compiler optimization to reference static data
        {
            -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
            -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
            -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, 62, -1, -1, -1, 63,         // 62 is placed at index 43 (for +), 63 at index 47 (for /)
            52, 53, 54, 55, 56, 57, 58, 59, 60, 61, -1, -1, -1, -1, -1, -1,         // 52-61 are placed at index 48-57 (for 0-9), 64 at index 61 (for =)
            -1, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14,
            15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, -1, -1, -1, -1, -1,         // 0-25 are placed at index 65-90 (for A-Z)
            -1, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40,
            41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, -1, -1, -1, -1, -1,         // 26-51 are placed at index 97-122 (for a-z)
            -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,         // Bytes over 122 ('z') are invalid and cannot be decoded
            -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,         // Hence, padding the map with 255, which indicates invalid input
            -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
            -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
            -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
            -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
            -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
            -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
        };
        
        private static ReadOnlySpan<byte> CharToHexLookup => new byte[]
        {
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, // 15
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, // 31
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, // 47
            0x0,  0x1,  0x2,  0x3,  0x4,  0x5,  0x6,  0x7,  0x8,  0x9,  0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, // 63
            0xFF, 0xA,  0xB,  0xC,  0xD,  0xE,  0xF,  0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, // 79
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, // 95
            0xFF, 0xa,  0xb,  0xc,  0xd,  0xe,  0xf,  0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, // 111
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, // 127
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, // 143
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, // 159
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, // 175
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, // 191
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, // 207
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, // 223
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, // 239
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF  // 255
        };

        #endregion

        #region Interface

        /// <summary>
        /// TryFromBase64String - convert base64 string to byte array
        /// </summary>
        /// <param name="s">string</param>
        /// <param name="bytes">result byte array</param>
        /// <param name="bytesWritten">bytes written</param>
        /// <returns></returns>
        public static bool TryFromBase64String(string s, Span<byte> bytes, out int bytesWritten)
        {
            if (s == null)
            {
                throw new ArgumentNullException(nameof(s));
            }

            return TryFromBase64Chars(s.AsSpan(), bytes, out bytesWritten);
        }

        #endregion

        #region Methods

        private static bool TryFromBase64Chars(ReadOnlySpan<char> chars, Span<byte> bytes, out int bytesWritten)
        {
            // This is actually local to one of the nested blocks but is being declared at the top as we don't want multiple stackallocs
            // for each iteraton of the loop.
            Span<char> tempBuffer = stackalloc char[4];  // Note: The tempBuffer size could be made larger than 4 but the size must be a multiple of 4.

            bytesWritten = 0;

            while (chars.Length != 0)
            {
                // Attempt to decode a segment that doesn't contain whitespace.
                bool complete = TryDecodeFromUtf16(chars, bytes, out int consumedInThisIteration, out int bytesWrittenInThisIteration);
                bytesWritten += bytesWrittenInThisIteration;
                if (complete)
                    return true;

                chars = chars.Slice(consumedInThisIteration);
                bytes = bytes.Slice(bytesWrittenInThisIteration);

                Debug.Assert(chars.Length != 0); // If TryDecodeFromUtf16() consumed the entire buffer, it could not have returned false.
                if (chars[0].IsSpace())
                {
                    // If we got here, the very first character not consumed was a whitespace. We can skip past any consecutive whitespace, then continue decoding.

                    int indexOfFirstNonSpace = 1;
                    while (true)
                    {
                        if (indexOfFirstNonSpace == chars.Length)
                            break;
                        if (!chars[indexOfFirstNonSpace].IsSpace())
                            break;
                        indexOfFirstNonSpace++;
                    }

                    chars = chars.Slice(indexOfFirstNonSpace);

                    if ((bytesWrittenInThisIteration % 3) != 0 && chars.Length != 0)
                    {
                        // If we got here, the last successfully decoded block encountered an end-marker, yet we have trailing non-whitespace characters.
                        // That is not allowed.
                        bytesWritten = default;
                        return false;
                    }

                    // We now loop again to decode the next run of non-space characters.
                }
                else
                {
                    Debug.Assert(chars.Length != 0 && !chars[0].IsSpace());

                    // If we got here, it is possible that there is whitespace that occurred in the middle of a 4-byte chunk. That is, we still have
                    // up to three Base64 characters that were left undecoded by the fast-path helper because they didn't form a complete 4-byte chunk.
                    // This is hopefully the rare case (multiline-formatted base64 message with a non-space character width that's not a multiple of 4.)
                    // We'll filter out whitespace and copy the remaining characters into a temporary buffer.
                    CopyToTempBufferWithoutWhiteSpace(chars, tempBuffer, out int consumedFromChars, out int charsWritten);
                    if ((charsWritten & 0x3) != 0)
                    {
                        // Even after stripping out whitespace, the number of characters is not divisible by 4. This cannot be a legal Base64 string.
                        bytesWritten = default;
                        return false;
                    }

                    tempBuffer = tempBuffer.Slice(0, charsWritten);
                    if (!TryDecodeFromUtf16(tempBuffer, bytes, out int consumedFromTempBuffer, out int bytesWrittenFromTempBuffer))
                    {
                        bytesWritten = default;
                        return false;
                    }
                    bytesWritten += bytesWrittenFromTempBuffer;
                    chars = chars.Slice(consumedFromChars);
                    bytes = bytes.Slice(bytesWrittenFromTempBuffer);

                    if ((bytesWrittenFromTempBuffer % 3) != 0)
                    {
                        // If we got here, this decode contained one or more padding characters ('='). We can accept trailing whitespace after this
                        // but nothing else.
                        for (int i = 0; i < chars.Length; i++)
                        {
                            if (!chars[i].IsSpace())
                            {
                                bytesWritten = default;
                                return false;
                            }
                        }
                        return true;
                    }

                    // We now loop again to decode the next run of non-space characters.
                }
            }

            return true;
        }

        private static bool TryDecodeFromUtf16(ReadOnlySpan<char> chars, Span<byte> bytes)
        {
            return TryDecodeFromUtf16(chars, bytes, out _);
        }

        private static int FromChar(int c)
        {
            return c >= CharToHexLookup.Length ? 0xFF : CharToHexLookup[c];
        }

        private static bool TryDecodeFromUtf16(ReadOnlySpan<char> chars, Span<byte> bytes, out int charsProcessed)
        {
            Debug.Assert(chars.Length % 2 == 0, "Un-even number of characters provided");
            Debug.Assert(chars.Length / 2 == bytes.Length, "Target buffer not right-sized for provided characters");

            int i = 0;
            int j = 0;
            int byteLo = 0;
            int byteHi = 0;
            while (j < bytes.Length)
            {
                byteLo = FromChar(chars[i + 1]);
                byteHi = FromChar(chars[i]);

                // byteHi hasn't been shifted to the high half yet, so the only way the bitwise or produces this pattern
                // is if either byteHi or byteLo was not a hex character.
                if ((byteLo | byteHi) == 0xFF)
                    break;

                bytes[j++] = (byte)((byteHi << 4) | byteLo);
                i += 2;
            }

            if (byteLo == 0xFF)
                i++;

            charsProcessed = i;
            return (byteLo | byteHi) != 0xFF;
        }

        private static bool TryDecodeFromUtf16(ReadOnlySpan<char> utf16, Span<byte> bytes, out int consumed, out int written)
        {
            ref char srcChars = ref MemoryMarshal.GetReference(utf16);
            ref byte destBytes = ref MemoryMarshal.GetReference(bytes);

            int srcLength = utf16.Length & ~0x3;  // only decode input up to the closest multiple of 4.
            int destLength = bytes.Length;

            int sourceIndex = 0;
            int destIndex = 0;

            if (utf16.Length == 0)
                goto DoneExit;

            ref sbyte decodingMap = ref MemoryMarshal.GetReference(DecodingMap);

            // Last bytes could have padding characters, so process them separately and treat them as valid.
            const int skipLastChunk = 4;

            int maxSrcLength;
            if (destLength >= (srcLength >> 2) * 3)
            {
                maxSrcLength = srcLength - skipLastChunk;
            }
            else
            {
                // This should never overflow since destLength here is less than int.MaxValue / 4 * 3 (i.e. 1610612733)
                // Therefore, (destLength / 3) * 4 will always be less than 2147483641
                maxSrcLength = (destLength / 3) * 4;
            }

            while (sourceIndex < maxSrcLength)
            {
                int result = Decode(ref Unsafe.Add(ref srcChars, sourceIndex), ref decodingMap);
                if (result < 0)
                    goto InvalidExit;
                WriteThreeLowOrderBytes(ref Unsafe.Add(ref destBytes, destIndex), result);
                destIndex += 3;
                sourceIndex += 4;
            }

            if (maxSrcLength != srcLength - skipLastChunk)
                goto InvalidExit;

            // If input is less than 4 bytes, srcLength == sourceIndex == 0
            // If input is not a multiple of 4, sourceIndex == srcLength != 0
            if (sourceIndex == srcLength)
            {
                goto InvalidExit;
            }

            int i0 = Unsafe.Add(ref srcChars, srcLength - 4);
            int i1 = Unsafe.Add(ref srcChars, srcLength - 3);
            int i2 = Unsafe.Add(ref srcChars, srcLength - 2);
            int i3 = Unsafe.Add(ref srcChars, srcLength - 1);
            if (((i0 | i1 | i2 | i3) & 0xffffff00) != 0)
                goto InvalidExit;

            i0 = Unsafe.Add(ref decodingMap, i0);
            i1 = Unsafe.Add(ref decodingMap, i1);

            i0 <<= 18;
            i1 <<= 12;

            i0 |= i1;

            if (i3 != EncodingPad)
            {
                i2 = Unsafe.Add(ref decodingMap, i2);
                i3 = Unsafe.Add(ref decodingMap, i3);

                i2 <<= 6;

                i0 |= i3;
                i0 |= i2;

                if (i0 < 0)
                    goto InvalidExit;
                if (destIndex > destLength - 3)
                    goto InvalidExit;
                WriteThreeLowOrderBytes(ref Unsafe.Add(ref destBytes, destIndex), i0);
                destIndex += 3;
            }
            else if (i2 != EncodingPad)
            {
                i2 = Unsafe.Add(ref decodingMap, i2);

                i2 <<= 6;

                i0 |= i2;

                if (i0 < 0)
                    goto InvalidExit;
                if (destIndex > destLength - 2)
                    goto InvalidExit;
                Unsafe.Add(ref destBytes, destIndex) = (byte)(i0 >> 16);
                Unsafe.Add(ref destBytes, destIndex + 1) = (byte)(i0 >> 8);
                destIndex += 2;
            }
            else
            {
                if (i0 < 0)
                    goto InvalidExit;
                if (destIndex > destLength - 1)
                    goto InvalidExit;
                Unsafe.Add(ref destBytes, destIndex) = (byte)(i0 >> 16);
                destIndex++;
            }

            sourceIndex += 4;

            if (srcLength != utf16.Length)
                goto InvalidExit;

            DoneExit:
            consumed = sourceIndex;
            written = destIndex;
            return true;

            InvalidExit:
            consumed = sourceIndex;
            written = destIndex;
            Debug.Assert((consumed % 4) == 0);
            return false;
        }

        private static void WriteThreeLowOrderBytes(ref byte destination, int value)
        {
            destination = (byte)(value >> 16);
            Unsafe.Add(ref destination, 1) = (byte)(value >> 8);
            Unsafe.Add(ref destination, 2) = (byte)value;
        }

        private static int Decode(ref char encodedChars, ref sbyte decodingMap)
        {
            int i0 = encodedChars;
            int i1 = Unsafe.Add(ref encodedChars, 1);
            int i2 = Unsafe.Add(ref encodedChars, 2);
            int i3 = Unsafe.Add(ref encodedChars, 3);

            if (((i0 | i1 | i2 | i3) & 0xffffff00) != 0)
                return -1; // One or more chars falls outside the 00..ff range. This cannot be a valid Base64 character.

            i0 = Unsafe.Add(ref decodingMap, i0);
            i1 = Unsafe.Add(ref decodingMap, i1);
            i2 = Unsafe.Add(ref decodingMap, i2);
            i3 = Unsafe.Add(ref decodingMap, i3);

            i0 <<= 18;
            i1 <<= 12;
            i2 <<= 6;

            i0 |= i3;
            i1 |= i2;

            i0 |= i1;
            return i0;
        }

        private static void CopyToTempBufferWithoutWhiteSpace(ReadOnlySpan<char> chars, Span<char> tempBuffer, out int consumed, out int charsWritten)
        {
            Debug.Assert(tempBuffer.Length != 0); // We only bound-check after writing a character to the tempBuffer.

            charsWritten = 0;
            for (int i = 0; i < chars.Length; i++)
            {
                char c = chars[i];
                if (!c.IsSpace())
                {
                    tempBuffer[charsWritten++] = c;
                    if (charsWritten == tempBuffer.Length)
                    {
                        consumed = i + 1;
                        return;
                    }
                }
            }
            consumed = chars.Length;
        }
        
        #endregion
    }
}
