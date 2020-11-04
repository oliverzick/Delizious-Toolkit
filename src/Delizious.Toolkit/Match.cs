﻿namespace Delizious
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Provides static factory methods to create <see cref="Match{T}"/> instances.
    /// </summary>
    public static class Match
    {
        /// <summary>
        /// Creates a <see cref="Match{T}"/> instance that always matches successfully regardless the value to match.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the value to match.
        /// </typeparam>
        /// <returns>
        /// A new <see cref="Match{T}"/> instance that always matches successfully.
        /// </returns>
        public static Match<T> Always<T>()
            => Match<T>.Create(PredefinedMatch<T>.Always());

        /// <summary>
        /// Creates a <see cref="Match{T}"/> instance that never matches successfully regardless the value to match.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the value to match.
        /// </typeparam>
        /// <returns>
        /// A new <see cref="Match{T}"/> instance that never matches successfully.
        /// </returns>
        public static Match<T> Never<T>()
            => Match<T>.Create(PredefinedMatch<T>.Never());

        private sealed class PredefinedMatch<T> : IMatch<T>
        {
            private readonly bool matches;

            private PredefinedMatch(bool matches)
            {
                this.matches = matches;
            }

            public static PredefinedMatch<T> Always()
                => new PredefinedMatch<T>(true);

            public static IMatch<T> Never()
                => new PredefinedMatch<T>(false);

            public bool Matches(T value)
                => this.matches;
        }

        /// <summary>
        /// Creates a <see cref="Match{T}"/> instance that matches successfully when the value to match is a <c>null</c> reference.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the value to match. This must be a reference type.
        /// </typeparam>
        /// <returns>
        /// A new <see cref="Match{T}"/> instance that matches successfully when the value to match is a <c>null</c> reference.
        /// </returns>
        public static Match<T> Null<T>() where T : class
            => Match<T>.Create(NullMatch<T>.Create());

        /// <summary>
        /// Creates a <see cref="Match{T}"/> instance that matches successfully when the value to match is not a <c>null</c> reference.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the value to match. This must be a reference type.
        /// </typeparam>
        /// <returns>
        /// A new <see cref="Match{T}"/> instance that matches successfully when the value to match is not a <c>null</c> reference.
        /// </returns>
        public static Match<T> NotNull<T>() where T : class
            => Match<T>.Create(NotMatch<T>.Create(NullMatch<T>.Create()));

        private sealed class NullMatch<T> : IMatch<T>
            where T : class
        {
            private NullMatch()
            {
            }

            public static NullMatch<T> Create()
                => new NullMatch<T>();

            public bool Matches(T value)
                => ReferenceEquals(value, null);
        }

        /// <summary>
        /// Creates a <see cref="Match{T}"/> instance that matches successfully when a value to match equals the specified <paramref name="reference"/> value.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the value to match.
        /// </typeparam>
        /// <param name="reference">
        /// The reference value a value to match must equal to match successfully.
        /// </param>
        /// <param name="equalityComparer">
        /// The <see cref="IEqualityComparer{T}"/> to determine whether a value to match and the <paramref name="reference"/> value are equal.
        /// </param>
        /// <returns>
        /// A new <see cref="Match{T}"/> instance that matches successfully when a value to match equals the specified <paramref name="reference"/> value.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="reference"/> is <c>null</c>. When matching an instance to be a <c>null</c> reference use <see cref="Null{T}"/> instead.</para>
        /// <para>- or -</para>
        /// <para><paramref name="equalityComparer"/> is <c>null</c>.</para>
        /// </exception>
        public static Match<T> Equal<T>([NotNull] T reference, [NotNull] IEqualityComparer<T> equalityComparer)
        {
            if (ReferenceEquals(reference, null))
            {
                throw new ArgumentNullException(nameof(reference));
            }

            if (ReferenceEquals(equalityComparer, null))
            {
                throw new ArgumentNullException(nameof(equalityComparer));
            }

            return Match<T>.Create(EqualityMatch<T>.Create(reference, equalityComparer));
        }

        /// <summary>
        /// Creates a <see cref="Match{T}"/> instance that matches successfully when a value to match does not equal the specified <paramref name="reference"/> value.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the value to match.
        /// </typeparam>
        /// <param name="reference">
        /// The reference value a value to match must not equal to match successfully.
        /// </param>
        /// <param name="equalityComparer">
        /// The <see cref="IEqualityComparer{T}"/> to determine whether a value to match and the <paramref name="reference"/> value are equal.
        /// </param>
        /// <returns>
        /// A new <see cref="Match{T}"/> instance that matches successfully when a value to match does not equal the specified <paramref name="reference"/> value.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="reference"/> is <c>null</c>.</para>
        /// <para>- or -</para>
        /// <para><paramref name="equalityComparer"/> is <c>null</c>.</para>
        /// </exception>
        public static Match<T> NotEqual<T>([NotNull] T reference, [NotNull] IEqualityComparer<T> equalityComparer)
        {
            if (ReferenceEquals(reference, null))
            {
                throw new ArgumentNullException(nameof(reference));
            }

            if (ReferenceEquals(equalityComparer, null))
            {
                throw new ArgumentNullException(nameof(equalityComparer));
            }

            return Match<T>.Create(NotMatch<T>.Create(EqualityMatch<T>.Create(reference, equalityComparer)));
        }

        private sealed class EqualityMatch<T> : IMatch<T>
        {
            private readonly T reference;
            private readonly IEqualityComparer<T> equalityComparer;

            private EqualityMatch(T reference, IEqualityComparer<T> equalityComparer)
            {
                this.reference = reference;
                this.equalityComparer = equalityComparer;
            }

            public static EqualityMatch<T> Create(T reference, IEqualityComparer<T> equalityComparer)
                => new EqualityMatch<T>(reference, equalityComparer);

            public bool Matches(T value)
                => this.equalityComparer.Equals(this.reference, value);
        }

        /// <summary>
        /// Creates a <see cref="Match{T}"/> instance that matches successfully when the value to match is the same instance as the specified <paramref name="reference"/> value.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the value to match. This must be a reference type.
        /// </typeparam>
        /// <param name="reference">
        /// The instance a value to match must be the same to match successfully.
        /// </param>
        /// <returns>
        /// A new <see cref="Match{T}"/> instance that matches successfully when the value to match is the same instance as the specified <paramref name="reference"/> value.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="reference"/> is <c>null</c>. When matching an instance to be a <c>null</c> reference use <see cref="Null{T}"/> instead.
        /// </exception>
        public static Match<T> Same<T>([NotNull] T reference) where T : class
        {
            if (ReferenceEquals(reference, null))
            {
                throw new ArgumentNullException(nameof(reference));
            }

            return Match<T>.Create(SameMatch<T>.Create(reference));
        }

        private sealed class SameMatch<T> : IMatch<T>
            where T : class
        {
            private readonly T reference;

            private SameMatch(T reference)
            {
                this.reference = reference;
            }

            public static SameMatch<T> Create(T reference)
                => new SameMatch<T>(reference);

            public bool Matches(T value)
                => ReferenceEquals(this.reference, value);
        }

        private sealed class NotMatch<T> : IMatch<T>
        {
            private readonly IMatch<T> match;

            private NotMatch(IMatch<T> match)
            {
                this.match = match;
            }

            public static NotMatch<T> Create(IMatch<T> match)
                => new NotMatch<T>(match);

            public bool Matches(T value)
                => !this.match.Matches(value);
        }
    }

    /// <summary>
    /// Represents a strongly typed match that provides a method to determine whether a value matches with.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the value to match.
    /// </typeparam>
    public sealed class Match<T> : IMatch<T>
    {
        private readonly IMatch<T> match;

        private Match(IMatch<T> match)
        {
            this.match = match;
        }

        internal static Match<T> Create(IMatch<T> match)
            => new Match<T>(match);

        /// <summary>
        /// Determines whether the specified <paramref name="value"/> successfully matches according to this match.
        /// </summary>
        /// <param name="value">
        /// The value to match.
        /// </param>
        /// <returns>
        /// <c>true</c> if <paramref name="value"/> successfully matches according to this match; otherwise, <c>false</c>.
        /// </returns>
        public bool Matches(T value)
            => this.match.Matches(value);
    }

    internal interface IMatch<in T>
    {
        bool Matches(T value);
    }
}
