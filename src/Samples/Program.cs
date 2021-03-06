﻿#region Copyright and license
// <copyright file="Program.cs">
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

[assembly: System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]

namespace Delizious
{
    using System;
    using System.Linq;

    internal class Program
    {
        static void Main(string[] args)
        {
            var samples = new (string Name, Action Action)[]
                          {
                              ("Match", MatchSample.Run),
                              ("Decision Tree", DecisionTreeSample.Run)
                          };

            foreach (var sample in samples)
            {
                var count = sample.Name.Length + 8;
                var delimiterLine = string.Concat(Enumerable.Repeat("=", count));

                Console.WriteLine(delimiterLine);
                Console.WriteLine($"=== {sample.Name} ===");
                Console.WriteLine(delimiterLine);

                sample.Action();

                Console.WriteLine();
            }
        }
    }
}
