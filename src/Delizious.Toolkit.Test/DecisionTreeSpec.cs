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
    using System.Linq;
    using Xunit;

    public sealed class DecisionTreeSpec
    {
        public sealed class Composite
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

        public sealed class Decision
        {
            [Fact]
            public void Throws_exception_when_match_is_null()
            {
                Assert.Throws<ArgumentNullException>(() => DecisionTree.Decision(null!, DecisionTree.Result<Context, string>("1")));
            }

            [Fact]
            public void Throws_exception_when_child_is_null()
            {
                Assert.Throws<ArgumentNullException>(() => DecisionTree.Decision<Context, string>(Match.Always<Context>(), null!));
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
        public void All__Evaluates_sequentially(DecisionTree<Context, string> subject, Context context, IEnumerable<string> expected)
        {
            var actual = subject.All(context);

            Assert.Equal(expected, actual);
        }

        public static IEnumerable<object[]> AllTheories()
        {
            // Result strategy
            yield return DataTheory(DecisionTree.Result<Context, string>(string.Empty),
                                    Context.Create(),
                                    MakeEnumerable(string.Empty));

            yield return DataTheory(DecisionTree.Result<Context, string>("Test"),
                                    Context.Create(),
                                    MakeEnumerable("Test"));

            // Decision strategy
            yield return DataTheory(DecisionTree.Decision(Match.Never<Context>(),
                                                          DecisionTree.Result<Context, string>("One")),
                                    Context.Create(),
                                    Enumerable.Empty<string>());

            yield return DataTheory(DecisionTree.Decision(Match.Always<Context>(),
                                                          DecisionTree.Result<Context, string>("One")),
                                    Context.Create(),
                                    MakeEnumerable("One"));

            yield return DataTheory(DecisionTree.Decision(Match.Transform<Context, int>(context => context.IntValue,
                                                                                        Match.Equal(123)),
                                                          DecisionTree.Result<Context, string>("One")),
                                    Context.Create(321),
                                    Enumerable.Empty<string>());

            yield return DataTheory(DecisionTree.Decision(Match.Transform<Context, int>(context => context.IntValue,
                                                                                        Match.Equal(123)),
                                                          DecisionTree.Result<Context, string>("One")),
                                    Context.Create(123),
                                    MakeEnumerable("One"));

            // Composite strategy
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

        [Theory]
        [MemberData(nameof(AllTheories__Integration))]
        public void All__Evaluates_sequentially__Integration(Context context, IEnumerable<string> expected)
        {
            var subject = AllSubject__Integration();

            var actual = subject.All(context);

            Assert.Equal(expected, actual);
        }

        private static DecisionTree<Context, string> AllSubject__Integration()
            => MakeComposite(MakeDecision(MakeMatch(c => c.IntValue,
                                                    Match.LessThan(0)),
                                          MakeComposite(MakeResult("One"),
                                                        MakeResult("Two"),
                                                        MakeDecision(MakeMatch(c => c.IntValue,
                                                                               Match.GreaterThan(-5)),
                                                                     MakeResult("Three")),
                                                        MakeResult("Four"),
                                                        MakeDecision(MakeMatch(c => c.BoolValue,
                                                                               Match.Equal(false)),
                                                                     MakeResult("Five")),
                                                        MakeResult("Six"))
                                         ),
                             MakeResult("Seven"),
                             MakeDecision(MakeMatch(c => c.BoolValue,
                                                    Match.Equal(true)),
                                          MakeComposite(MakeDecision(MakeMatch(c => c.IntValue,
                                                                               Match.GreaterThanOrEqualTo(-2)),
                                                                     MakeResult("Eight")),
                                                        MakeResult("Nine"))),
                             MakeDecision(Match.All(MakeMatch(c => c.BoolValue,
                                                              Match.Equal(true)),
                                                    MakeMatch(c => c.IntValue,
                                                              Match.Equal(0))),
                                          MakeResult("Ten"))
                            );

        public static IEnumerable<object[]> AllTheories__Integration()
        {
            yield return DataTheory(Context.Create(-5, false), MakeEnumerable("One", "Two", "Four", "Five", "Six", "Seven"));
            yield return DataTheory(Context.Create(-5, true), MakeEnumerable("One", "Two", "Four", "Six", "Seven", "Nine"));

            yield return DataTheory(Context.Create(-4, false), MakeEnumerable("One", "Two", "Three", "Four", "Five", "Six", "Seven"));
            yield return DataTheory(Context.Create(-4, true), MakeEnumerable("One", "Two", "Three", "Four", "Six", "Seven", "Nine"));

            yield return DataTheory(Context.Create(-1, false), MakeEnumerable("One", "Two", "Three", "Four", "Five", "Six", "Seven"));
            yield return DataTheory(Context.Create(-1, true), MakeEnumerable("One", "Two", "Three", "Four", "Six", "Seven", "Eight", "Nine"));

            yield return DataTheory(Context.Create(0, false), MakeEnumerable("Seven"));
            yield return DataTheory(Context.Create(0, true), MakeEnumerable("Seven", "Eight", "Nine", "Ten"));
        }

        private static object[] DataTheory(params object[] values)
            => values;

        private static T[] MakeEnumerable<T>(params T[] items)
            => items;
        private static DecisionTree<Context, T> MakeComposite<T>(params DecisionTree<Context, T>[] children)
            => DecisionTree.Composite(children);

        private static DecisionTree<Context, T> MakeDecision<T>(Match<Context> match, DecisionTree<Context, T> child)
            => DecisionTree.Decision(match, child);

        private static DecisionTree<Context, T> MakeResult<T>(T result)
            => DecisionTree.Result<Context, T>(result);

        private static Match<Context> MakeMatch<T>(Func<Context, T> transformation, Match<T> match)
            => Match.Transform(transformation, match);

        public sealed class Context
        {
            private Context(int intValue, bool boolValue)
            {
                this.IntValue = intValue;
                this.BoolValue = boolValue;
            }

            internal static Context Create(int intValue = default, bool boolValue = default)
                => new Context(intValue, boolValue);

            public int IntValue { get; }

            public bool BoolValue { get; }
        }
    }
}
