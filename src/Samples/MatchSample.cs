﻿#region Copyright and license
// <copyright file="MatchSample.cs">
//     Copyright (c) 2020-2021 Oliver Zick. All rights reserved.
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

namespace Delizious
{
    using System;
    using System.Linq;

    internal sealed class MatchSample
    {
        internal static void Run()
        {
            // Match when value <= -1 or value == 2 or (value => 4 and value < 7) or value > 10
            var match = Match.Any(Match.LessThanOrEqualTo(-1),
                                  Match.EqualTo(2),
                                  Match.All(Match.GreaterThanOrEqualTo(4),
                                            Match.LessThan(7)),
                                  Match.GreaterThan(10));

            var values = Enumerable.Range(-5, 20);

            Console.WriteLine("Match when value <= -1 or value == 2 or (value => 4 and value < 7) or value > 10");

            foreach (var value in values)
            {
                var result = match.Matches(value);

                Console.WriteLine($"Match value {value}: {result}");
            }
        }
    }
}
