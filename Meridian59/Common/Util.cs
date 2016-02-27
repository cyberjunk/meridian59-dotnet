/*
 Copyright (c) 2012-2013 Clint Banzhaf
 This file is part of "Meridian59 .NET".

 "Meridian59 .NET" is free software: 
 You can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, 
 either version 3 of the License, or (at your option) any later version.

 "Meridian59 .NET" is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY;
 without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 See the GNU General Public License for more details.

 You should have received a copy of the GNU General Public License along with "Meridian59 .NET".
 If not, see http://www.gnu.org/licenses/.
*/

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Meridian59.Native;

namespace Meridian59.Common
{
    /// <summary>
    /// General utility functions
    /// </summary>
    public static class Util
    {
        /// <summary>
        /// Loads file content from filesystem to unmanaged memory.
        /// Beware: This does not verify existance of file 
        /// and does not care about freeing memory again.
        /// </summary>
        /// <param name="File">Full filename (including path and extension)</param>
        /// <returns>Memory pointer and length of loaded data</returns>
        public static Tuple<IntPtr, uint> LoadFileToUnmanagedMem(string File)
        {
            // load it                    
            byte[] bytes = System.IO.File.ReadAllBytes(File);

            // allocate unmanaged memory ( so GC doesn't move this around )
            IntPtr ptr = Marshal.AllocHGlobal(bytes.Length);
            uint len = (uint)bytes.Length;

            // copy to unmanaged
            Wrapper.CopyMem(bytes, 0, ptr, len);

            // return info
            return new Tuple<IntPtr, uint>(ptr, len);
        }

        /// <summary>
        /// Returns each byte of a 32-Bit integer.
        /// </summary>
        /// <param name="Value"></param>
        /// <param name="B1">1. Byte</param>
        /// <param name="B2">2. Byte</param>
        /// <param name="B3">3. Byte</param>
        /// <param name="B4">4. Byte</param>
        public static void GetIntegerBytes(uint Value, out byte B1, out byte B2, out byte B3, out byte B4)
        {
            B1 = (byte)((Value & 0xFF000000) >> 24);
            B2 = (byte)((Value & 0x00FF0000) >> 16);
            B3 = (byte)((Value & 0x0000FF00) >> 8);
            B4 = (byte)((Value & 0x000000FF));
        }

        /// <summary>
        /// Searches for all offsets of SearchPattern in Data.
        /// </summary>
        /// <param name="SearchPattern">Search for this</param>
        /// <param name="Data">Search in this</param>
        /// <returns>A list of all found offsets, can be empty.</returns>
        public static List<int> FindSequences(byte?[] SearchPattern, byte[] Data)
        {
            // saves offsets we found
            List<int> offsets = new List<int>();

            // searchpattern must be at least the length of data
            if (SearchPattern != null && Data != null && Data.Length >= SearchPattern.Length)
            {
                // walk bytes in data
                for (int i = 0; i < Data.Length - SearchPattern.Length; i++)
                {
                    // compare with bytes in pattern
                    for (int j = 0; j < SearchPattern.Length; j++)
                    {
                        // skip iteration if null (wildcard) and not last
                        if (SearchPattern[j] == null && j < SearchPattern.Length - 1)
                            continue;

                        // go next offset if mismatch
                        else if (SearchPattern[j] != null && Data[i + j] != SearchPattern[j])
                            break;

                        // all matched = found
                        if (j == SearchPattern.Length - 1)
                            offsets.Add(i);
                    }
                }
            }

            return offsets;
        }

        /// <summary>
        /// Searches for Pattern in Data.
        /// Uses FindSequences.
        /// </summary>
        /// <param name="Pattern"></param>
        /// <param name="Data"></param>
        /// <param name="Orig"></param>
        /// <param name="BaseAddress"></param>
        /// <returns>Address if exactly one found, otherwise IntPtr.Zero</returns>
        public static IntPtr Find(byte?[] Pattern, byte[] Data, out byte[] Orig, int BaseAddress = 0)
        {
            // search for offsets of PATTERN
            List<int> offsets = FindSequences(Pattern, Data);

            // handle results
            if (offsets.Count == 1)
            {
                // save offset
                IntPtr address = (IntPtr)(offsets[0] + BaseAddress);

                // save original bytecode
                Orig = new byte[Pattern.Length];
                Array.Copy(Data, offsets[0], Orig, 0, Pattern.Length);

                return address;
            }
            else
            {
                // empty orig bytecode
                Orig = new byte[0];

                return IntPtr.Zero;
            }
        }

        /// <summary>
        /// Counts occurences of string pattern in data.
        /// </summary>
        /// <remarks>
        /// Example:
        /// --------
        ///  Data    = aaaa
        ///  Pattern = aa
        ///  
        /// If SkipSubstringLengthOnMatch is true,
        /// then this function will return 2 occurences.
        /// At index 0 and 2.
        /// 
        /// If set to false it returns 3 occurences.
        /// At index 0, 1 and 2.
        /// </remarks>
        /// <param name="Pattern">Search for this</param>
        /// <param name="Data">Search in this</param>
        /// <param name="SkipSubstringLengthOnMatch">See remarks</param>
        /// <returns>Amount of occurences</returns>
        public static int CountSubStringOccurences(string Pattern, string Data, bool SkipSubstringLengthOnMatch = true)
        {
            if (Pattern == null || Data == null)
                return 0;

            int count = 0;
            int n = 0;

            if (SkipSubstringLengthOnMatch)
            {
                while ((n = Data.IndexOf(Pattern, n, StringComparison.InvariantCulture)) != -1)
                {
                    n += Pattern.Length;
                    count++;
                }
            }
            else
            {
                while ((n = Data.IndexOf(Pattern, n, StringComparison.InvariantCulture)) != -1)
                {
                    n++;
                    count++;
                }
            }

            return count;
        }
       
        /// <summary>
        /// Tries to find a quote pattern like "some text" in a string.
        /// </summary>
        /// <param name="Text"></param>
        /// <param name="StartIndex"></param>
        /// <param name="QuoteChar"></param>
        /// <returns>
        /// First value is index of first quotechar, second value is full length (including both quotechars), 
        /// third is extracted quote string without quote chars. Returns NULL if no quote found.
        /// </returns>
        public static Tuple<int, int, string> GetQuote(this string Text, int StartIndex = 0, char QuoteChar = '\"')
        {
            int quote1, quote2, len;
            string quote;

            // no quote for null or if startindex beyond last
            if (Text == null || StartIndex >= Text.Length)
                return null;

            // get index of first quote
            quote1 = Text.IndexOf(QuoteChar, StartIndex);
            
            // not even one quote
            if (quote1 < 0)
                return null;

            // must have at least one more char for empy quote
            if (Text.Length <= quote1 + 1)
                return null;

            // get index of second quote
            quote2 = Text.IndexOf(QuoteChar, quote1 + 1);

            // no second quote
            if (quote2 < 0)
                return null;

            // get length and extract quote
            len = quote2 - quote1 + 1;
            quote = Text.Substring(quote1 + 1, len - 2);

            return new Tuple<int, int, string>(quote1, len, quote);
        }

        /// <summary>
        /// Replaces the first instance of a search string in the string with another string.
        /// </summary>
        /// <param name="Text"></param>
        /// <param name="Search"></param>
        /// <param name="Replace"></param>
        /// <returns></returns>
        public static string ReplaceFirst(this string Text, string Search, string Replace)
        {
            int position = Text.IndexOf(Search);
            if (position < 0)
                return Text;

            return Text.Substring(0, position) + Replace + Text.Substring(position + Search.Length);
        }

        /// <summary>
        /// Replaces the first instance of a search string in the string with another string.
        /// Starts searching at startPos.
        /// </summary>
        /// <param name="Text"></param>
        /// <param name="Search"></param>
        /// <param name="Replace"></param>
        /// <param name="StartPos"></param>
        /// <returns></returns>
        public static string ReplaceFirst(this string Text, string Search, string Replace, int StartPos)
        {
            int position = Text.IndexOf(Search, StartPos);
            if (position < 0)
                return Text;

            return Text.Substring(0, position) + Replace + Text.Substring(position + Search.Length);
        }
    }
}
