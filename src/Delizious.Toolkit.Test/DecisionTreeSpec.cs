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
        public sealed class Leaf
        {
            [Fact]
            public void Throws_exception_when_result_is_null()
            {
                Assert.Throws<ArgumentNullException>(() => DecisionTree.Leaf<Context, string>(null!));
            }
        }

        [Fact]
        public void All__Throws_exception_when_context_is_null()
        {
            var subject = DecisionTree.Leaf<Context, string>("Test");

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
            yield return DataTheory(DecisionTree.Leaf<Context, string>(string.Empty),
                                    Context.Create(),
                                    MakeEnumerable(string.Empty));

            yield return DataTheory(DecisionTree.Leaf<Context, string>("Test"),
                                    Context.Create(),
                                    MakeEnumerable("Test"));
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
