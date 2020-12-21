#region Copyright and license
// <copyright file="MatchSamples.cs">
//     Copyright (c) 2020 Oliver Zick. All rights reserved.
// </copyright>
// <author>Oliver Zick</author>
// <license>
//     MIT License
// 
//     Permission is hereby granted, free of charge, to any person obtaining a copy
//     of this software and associated documentation files (the "Software"), to deal
//     in the Software without restriction, including without limitation the rights
//     to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//     copies of the Software, and to permit persons to whom the Software is
//     furnished to do so, subject to the following conditions:
// 
//     The above copyright notice and this permission notice shall be included in all
//     copies or substantial portions of the Software.
// 
//     THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//     IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//     FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//     AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//     LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//     OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//     SOFTWARE.
// </license>
#endregion

using System;
using System.Collections.Generic;
using System.Linq;

namespace Delizious
{
    internal static class MatchSamples
    {
        public static void Run()
        {
            Always();
            Never();
        }

        public static void Always()
        {
            static IEnumerable<short> EnumerateShort()
            {
                for (var value = short.MinValue; value < short.MaxValue; value++)
                {
                    yield return value;
                }
            }

            var match = Match.Always<short>();

            // Match all short values, result must be true because all matches must be true
            var result = EnumerateShort().All(value => match.Matches(value));

            Console.WriteLine($"Match all short values with '{nameof(Match.Always)}' match: {result}");
        }

        public static void Never()
        {
            static IEnumerable<short> EnumerateShort()
            {
                for (var value = short.MinValue; value < short.MaxValue; value++)
                {
                    yield return value;
                }
            }

            var match = Match.Never<short>();

            // Match all short values, result must be false because any match must not be true
            var result = EnumerateShort().Any(value => match.Matches(value));

            Console.WriteLine($"Match all short values with '{nameof(Match.Never)}' match: {result}");
        }
    }
}
