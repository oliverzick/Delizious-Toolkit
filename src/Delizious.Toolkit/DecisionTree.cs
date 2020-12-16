﻿#region Copyright and license
// <copyright file="DecisionTree.cs">
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
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    /// <summary>
    /// Provides static factory methods to create <see cref="DecisionTree{TContext,TResult}"/> instances.
    /// </summary>
    public static class DecisionTree
    {
        public static DecisionTree<TContext, TResult> Composite<TContext, TResult>([NotNull] params DecisionTree<TContext, TResult>[] children)
        {
            if (children == null!)
            {
                throw new ArgumentNullException(nameof(children));
            }

            if (children.Any(child => ReferenceEquals(child, null!)))
            {
                throw new ArgumentException("At least one child is a null reference.", nameof(children));
            }

            return DecisionTree<TContext, TResult>.Composite(children);
        }

        public static DecisionTree<TContext, TResult> Decision<TContext, TResult>([NotNull] Match<TContext> match, [NotNull] DecisionTree<TContext, TResult> child)
        {
            if (ReferenceEquals(match, null!))
            {
                throw new ArgumentNullException(nameof(match));
            }

            if (ReferenceEquals(child, null!))
            {
                throw new ArgumentNullException(nameof(child));
            }

            return DecisionTree<TContext, TResult>.Decision(match, child);
        }

        public static DecisionTree<TContext, TResult> Result<TContext, TResult>([NotNull] TResult result)
        {
            if (ReferenceEquals(result, null!))
            {
                throw new ArgumentNullException(nameof(result));
            }

            return DecisionTree<TContext, TResult>.Result(result);
        }
    }

    public sealed class DecisionTree<TContext, TResult>
    {
        private readonly IStrategy strategy;

        private DecisionTree(IStrategy strategy)
        {
            this.strategy = strategy;
        }

        private static DecisionTree<TContext, TResult> Create(IStrategy strategy)
            => new DecisionTree<TContext, TResult>(strategy);

        internal static DecisionTree<TContext, TResult> Composite(IEnumerable<DecisionTree<TContext, TResult>> children)
            => Create(CompositeStrategy.Create(children.Select(child => child.strategy).ToArray()));

        internal static DecisionTree<TContext, TResult> Decision(Match<TContext> match, DecisionTree<TContext, TResult> child)
            => Create(DecisionStrategy.Create(match, child.strategy));

        internal static DecisionTree<TContext, TResult> Result(TResult result)
            => Create(ResultStrategy.Create(result));

        public IEnumerable<TResult> All([NotNull] TContext context)
        {
            if (ReferenceEquals(context, null!))
            {
                throw new ArgumentNullException(nameof(context));
            }

            return this.strategy.Decide(context);
        }

        private interface IStrategy
        {
            IEnumerable<TResult> Decide(TContext context);
        }

        private sealed class CompositeStrategy : IStrategy
        {
            private readonly IStrategy[] children;

            private CompositeStrategy(IStrategy[] children)
            {
                this.children = children;
            }

            public static CompositeStrategy Create(IStrategy[] children)
                => new CompositeStrategy(children);

            public IEnumerable<TResult> Decide(TContext context)
                => this.children.SelectMany(child => child.Decide(context));
        }

        private sealed class DecisionStrategy : IStrategy
        {
            private readonly Match<TContext> match;

            private readonly IStrategy child;

            private DecisionStrategy(Match<TContext> match, IStrategy child)
            {
                this.match = match;
                this.child = child;
            }

            public static DecisionStrategy Create(Match<TContext> match, IStrategy child)
                => new DecisionStrategy(match, child);

            public IEnumerable<TResult> Decide(TContext context)
                => Enumerable.Repeat(this.child, Convert.ToInt32(this.match.Matches(context)))
                             .SelectMany(item => item.Decide(context));
        }

        private sealed class ResultStrategy : IStrategy
        {
            private readonly TResult result;

            private ResultStrategy(TResult result)
            {
                this.result = result;
            }

            public static ResultStrategy Create(TResult result)
                => new ResultStrategy(result);

            public IEnumerable<TResult> Decide(TContext context)
            {
                yield return this.result;
            }
        }
    }
}
