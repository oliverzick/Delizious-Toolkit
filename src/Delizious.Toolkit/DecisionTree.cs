#region Copyright and license
// <copyright file="DecisionTree.cs">
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
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    /// <summary>
    /// Provides static factory methods to create <see cref="DecisionTree{TContext,TResult}"/> instances.
    /// </summary>
    public static class DecisionTree
    {
        /// <summary>
        /// Creates a <see cref="DecisionTree{TContext,TResult}"/> instance that represents a decision tree's composite node
        /// containing the given <paramref name="children"/>.
        /// </summary>
        /// <typeparam name="TContext">
        /// The type of the decision context that is used to make decision.
        /// </typeparam>
        /// <typeparam name="TResult">
        /// The type of the result this decision tree provides after making decisions.
        /// </typeparam>
        /// <param name="children">
        /// The child nodes of this composite node.
        /// </param>
        /// <returns>
        /// A new <see cref="DecisionTree{TContext,TResult}"/> instance that represents a decision tree's composite node.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///<paramref name="children"/> is <c>null</c>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="children"/> contain at least one child that is <c>null</c>.
        /// </exception>
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

        /// <summary>
        /// Creates a <see cref="DecisionTree{TContext,TResult}"/> instance that represents a decision tree's decision node
        /// which uses the given <paramref name="match"/> to make a decision and traverses the given <paramref name="child"/>
        /// when its decision is fulfilled.
        /// </summary>
        /// <typeparam name="TContext">
        /// The type of the decision context that is used to make decision.
        /// </typeparam>
        /// <typeparam name="TResult">
        /// The type of the result this decision tree provides after making decisions.
        /// </typeparam>
        /// <param name="match">
        /// The match that represents the decision of this node.
        /// </param>
        /// <param name="child">
        /// The child node this node traverses after making a decision.
        /// </param>
        /// <returns>
        /// A new <see cref="DecisionTree{TContext,TResult}"/> instance that represents a decision tree's decision node.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="match"/> is <c>null</c>.</para>
        /// <para>- or -</para>
        /// <para><paramref name="child"/> is <c>null</c>.</para>
        /// </exception>
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

        /// <summary>
        /// Creates a <see cref="DecisionTree{TContext,TResult}"/> instance that represents a decision tree's result node
        /// with the given <paramref name="result"/>.
        /// </summary>
        /// <typeparam name="TContext">
        /// The type of the decision context that is used to make decision.
        /// </typeparam>
        /// <typeparam name="TResult">
        /// The type of the result this decision tree provides after making decisions.
        /// </typeparam>
        /// <param name="result">
        /// The result.
        /// </param>
        /// <returns>
        /// A new <see cref="DecisionTree{TContext,TResult}"/> instance that represents a decision tree's result node.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///<paramref name="result"/> is <c>null</c>.
        /// </exception>
        public static DecisionTree<TContext, TResult> Result<TContext, TResult>([NotNull] TResult result)
        {
            if (ReferenceEquals(result, null!))
            {
                throw new ArgumentNullException(nameof(result));
            }

            return DecisionTree<TContext, TResult>.Result(result);
        }
    }

    /// <summary>
    /// Represents a decision tree that makes decisions using a given <see cref="TContext"/> instance
    /// and provides <see cref="TResult"/> instances after making decisions.
    /// When making decisions this decision tree traverses the given nodes sequentially from parent to child according to the order of declaration.
    /// </summary>
    /// <typeparam name="TContext">
    /// The type of the decision context that is used to make decision.
    /// </typeparam>
    /// <typeparam name="TResult">
    /// The type of the result this decision tree provides after making decisions.
    /// </typeparam>
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

        /// <summary>
        /// Traverses this decision tree and returns all results that fulfill its decisions according to the given <paramref name="context"/>.
        /// </summary>
        /// <param name="context">
        /// The context that is used to make decisions.
        /// </param>
        /// <returns>
        /// All results of this decision tree that fulfill its decision.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="context"/> is <c>null</c>.
        /// </exception>
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
