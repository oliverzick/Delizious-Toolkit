#region Copyright and license
// <copyright file="DecisionTreeSpec.cs">
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

namespace Delizious
{
    using System;
    using System.Collections.Generic;
    using Xunit;

    public sealed class DecisionTreeSpec
    {
        public class Composite
        {
            [Fact]
            public void Throws_exception_when_children_are_null()
            {
                Assert.Throws<ArgumentNullException>(() => DecisionTree.Composite<Context, string>(null!));
            }

            [Fact]
            public void Throws_exception_when_children_contain_null()
            {
                Assert.Throws<ArgumentException>(() => DecisionTree.Composite(DecisionTree.Result<Context, string>("1"),
                                                                              null!,
                                                                              DecisionTree.Result<Context, string>("2")));
            }
        }

        public sealed class Result
        {
            [Fact]
            public void Throws_exception_when_result_is_null()
            {
                Assert.Throws<ArgumentNullException>(() => DecisionTree.Result<Context, string>(null!));
            }
        }

        [Fact]
        public void All__Throws_exception_when_context_is_null()
        {
            var subject = DecisionTree.Result<Context, string>("Test");

            Assert.Throws<ArgumentNullException>(() => subject.All(null!));
        }

        [Theory]
        [MemberData(nameof(AllTheories))]
        public void All(DecisionTree<Context, string> subject, Context context, IEnumerable<string> expected)
        {
            var actual = subject.All(context);

            Assert.Equal(expected, actual);
        }

        public static IEnumerable<object[]> AllTheories()
        {
            yield return DataTheory(DecisionTree.Result<Context, string>(string.Empty),
                                    Context.Create(),
                                    MakeEnumerable(string.Empty));

            yield return DataTheory(DecisionTree.Result<Context, string>("Test"),
                                    Context.Create(),
                                    MakeEnumerable("Test"));

            yield return DataTheory(DecisionTree.Composite(DecisionTree.Result<Context, string>("One")),
                                    Context.Create(),
                                    MakeEnumerable("One"));

            yield return DataTheory(DecisionTree.Composite(DecisionTree.Result<Context, string>("One"),
                                                           DecisionTree.Result<Context, string>("Two"),
                                                           DecisionTree.Result<Context, string>("Three")),
                                    Context.Create(),
                                    MakeEnumerable("One", "Two", "Three"));

            yield return DataTheory(DecisionTree.Composite(DecisionTree.Composite(DecisionTree.Result<Context, string>("One"),
                                                                                  DecisionTree.Result<Context, string>("Two"),
                                                                                  DecisionTree.Result<Context, string>("Three")),
                                                           DecisionTree.Result<Context, string>("Four"),
                                                           DecisionTree.Composite(DecisionTree.Result<Context, string>("Five"),
                                                                                  DecisionTree.Result<Context, string>("Six"))),
                                    Context.Create(),
                                    MakeEnumerable("One", "Two", "Three", "Four", "Five", "Six"));
        }

        private static object[] DataTheory(params object[] values)
            => values;

        private static T[] MakeEnumerable<T>(params T[] items)
            => items;

        public sealed class Context
        {
            private Context()
            {
            }

            internal static Context Create()
                => new Context();
        }
    }
}
