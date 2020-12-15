#region Copyright and license
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

    /// <summary>
    /// Provides static factory methods to create <see cref="DecisionTree{TContext,TResult}"/> instances.
    /// </summary>
    public static class DecisionTree
    {
        public static DecisionTree<TContext, TResult> Leaf<TContext, TResult>([NotNull] TResult result)
        {
            if (ReferenceEquals(result, null!))
            {
                throw new ArgumentNullException(nameof(result));
            }

            return DecisionTree<TContext, TResult>.Leaf(result);
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

        internal static DecisionTree<TContext, TResult> Leaf(TResult result)
            => Create(LeafStrategy.Create(result));

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

        private sealed class LeafStrategy : IStrategy
        {
            private readonly TResult result;

            private LeafStrategy(TResult result)
            {
                this.result = result;
            }

            public static LeafStrategy Create(TResult result)
                => new LeafStrategy(result);

            public IEnumerable<TResult> Decide(TContext context)
            {
                yield return this.result;
            }
        }
    }
}
