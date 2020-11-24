namespace Delizious
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    /// <summary>
    /// Provides static factory methods to create <see cref="Match{T}"/> instances.
    /// </summary>
    public static class Match
    {
        /// <summary>
        /// Creates a <see cref="Match{T}"/> instance that always matches successfully regardless a value to match.
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
        /// Creates a <see cref="Match{T}"/> instance that never matches successfully regardless a value to match.
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
        /// Creates a <see cref="Match{T}"/> instance that matches successfully when a value to match is a <c>null</c> reference.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the value to match. This must be a reference type.
        /// </typeparam>
        /// <returns>
        /// A new <see cref="Match{T}"/> instance that matches successfully when a value to match is a <c>null</c> reference.
        /// </returns>
        public static Match<T> Null<T>() where T : class
            => Match<T>.Create(NullMatch<T>.Create());

        /// <summary>
        /// Creates a <see cref="Match{T}"/> instance that matches successfully when a value to match is not a <c>null</c> reference.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the value to match. This must be a reference type.
        /// </typeparam>
        /// <returns>
        /// A new <see cref="Match{T}"/> instance that matches successfully when a value to match is not a <c>null</c> reference.
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
        /// Creates a <see cref="Match{T}"/> instance that matches successfully when a value to match is the same instance as the specified <paramref name="reference"/> value.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the value to match. This must be a reference type.
        /// </typeparam>
        /// <param name="reference">
        /// The instance a value to match must be the same to match successfully.
        /// </param>
        /// <returns>
        /// A new <see cref="Match{T}"/> instance that matches successfully when a value to match is the same instance as the specified <paramref name="reference"/> value.
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

        /// <summary>
        /// Creates a <see cref="Match{T}"/> instance that matches successfully when a value to match is not the same instance as the specified <paramref name="reference"/> value.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the value to match. This must be a reference type.
        /// </typeparam>
        /// <param name="reference">
        /// The instance a value to match must not be the same to match successfully.
        /// </param>
        /// <returns>
        /// A new <see cref="Match{T}"/> instance that matches successfully when a value to match is not the same instance as the specified <paramref name="reference"/> value.
        /// </returns
        /// <exception cref="ArgumentNullException">
        /// <paramref name="reference"/> is <c>null</c>. When matching an instance to be a non-<c>null</c> reference use <see cref="NotNull{T}"/> instead.
        /// </exception>
        public static Match<T> NotSame<T>([NotNull] T reference) where T : class
        {
            if (ReferenceEquals(reference, null))
            {
                throw new ArgumentNullException(nameof(reference));
            }

            return Match<T>.Create(NotMatch<T>.Create(SameMatch<T>.Create(reference)));
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

        /// <summary>
        /// Creates a <see cref="Match{T}"/> instance that matches successfully when a value to match equals the specified <paramref name="reference"/> value.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the value to match that must implement the <see cref="IEquatable{T}"/> interface.
        /// </typeparam>
        /// <param name="reference">
        /// The reference value a value to match must equal to match successfully.
        /// </param>
        /// <returns>
        /// A new <see cref="Match{T}"/> instance that matches successfully when a value to match equals the specified <paramref name="reference"/> value.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="reference"/> is <c>null</c>. When matching an instance to be a <c>null</c> reference use <see cref="Null{T}"/> instead.
        /// </exception>
        public static Match<T> Equal<T>([NotNull] T reference)
            where T : IEquatable<T>
        {
            if (ReferenceEquals(reference, null))
            {
                throw new ArgumentNullException(nameof(reference));
            }

            return Match<T>.Create(EqualityMatch<T>.Create(reference));
        }

        /// <summary>
        /// Creates a <see cref="Match{T}"/> instance that matches successfully when a value to match does not equal the specified <paramref name="reference"/> value.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the value to match that must implement the <see cref="IEquatable{T}"/> interface.
        /// </typeparam>
        /// <param name="reference">
        /// The reference value a value to match must not equal to match successfully.
        /// </param>
        /// <returns>
        /// A new <see cref="Match{T}"/> instance that matches successfully when a value to match does not equal the specified <paramref name="reference"/> value.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="reference"/> is <c>null</c>. When matching an instance to be a non-<c>null</c> reference use <see cref="NotNull{T}"/> instead.
        /// </exception>
        public static Match<T> NotEqual<T>([NotNull] T reference)
            where T : IEquatable<T>
        {
            if (ReferenceEquals(reference, null))
            {
                throw new ArgumentNullException(nameof(reference));
            }

            return Match<T>.Create(NotMatch<T>.Create(EqualityMatch<T>.Create(reference)));
        }

        private sealed class EqualityMatch<T> : IMatch<T>
            where T : IEquatable<T>
        {
            private readonly T reference;

            private EqualityMatch(T reference)
            {
                this.reference = reference;
            }

            public static EqualityMatch<T> Create(T reference)
                => new EqualityMatch<T>(reference);

            public bool Matches(T value)
                => this.reference.Equals(value);
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

            return Match<T>.Create(EqualityComparisonMatch<T>.Create(reference, equalityComparer));
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
        /// <para><paramref name="reference"/> is <c>null</c>. When matching an instance to be a non-<c>null</c> reference use <see cref="NotNull{T}"/> instead.</para>
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

            return Match<T>.Create(NotMatch<T>.Create(EqualityComparisonMatch<T>.Create(reference, equalityComparer)));
        }

        private sealed class EqualityComparisonMatch<T> : IMatch<T>
        {
            private readonly T reference;
            private readonly IEqualityComparer<T> equalityComparer;

            private EqualityComparisonMatch(T reference, IEqualityComparer<T> equalityComparer)
            {
                this.reference = reference;
                this.equalityComparer = equalityComparer;
            }

            public static EqualityComparisonMatch<T> Create(T reference, IEqualityComparer<T> equalityComparer)
                => new EqualityComparisonMatch<T>(reference, equalityComparer);

            public bool Matches(T value)
                => this.equalityComparer.Equals(this.reference, value);
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
        /// <returns>
        /// A new <see cref="Match{T}"/> instance that matches successfully when a value to match equals the specified <paramref name="reference"/> value.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="reference"/> is <c>null</c>.
        /// </exception>
        public static Match<T> EqualTo<T>([NotNull] T reference)
            where T : IComparable<T>
        {
            if (ReferenceEquals(reference, null))
            {
                throw new ArgumentNullException(nameof(reference));
            }

            return Match<T>.Create(ComparisonMatch<T>.EqualTo(Comparison.Comparable(reference)));
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
        /// <param name="comparer">
        /// The <see cref="IComparer{T}"/> instance to determine whether a value to match equals the specified <paramref name="reference"/> value.
        /// </param>
        /// <returns>
        /// A new <see cref="Match{T}"/> instance that matches successfully when a value to match equals the specified <paramref name="reference"/> value.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="reference"/> is <c>null</c>.</para>
        /// <para>- or -</para>
        /// <para><paramref name="comparer"/> is <c>null</c>.</para>
        /// </exception>
        public static Match<T> EqualTo<T>([NotNull] T reference, [NotNull] IComparer<T> comparer)
        {
            if (ReferenceEquals(reference, null))
            {
                throw new ArgumentNullException(nameof(reference));
            }

            if (ReferenceEquals(comparer, null))
            {
                throw new ArgumentNullException(nameof(comparer));
            }

            return Match<T>.Create(CompareMatch<T>.EqualTo(reference, comparer));
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
        /// <param name="comparer">
        /// The <see cref="IComparer{T}"/> to determine whether a value to match and the <paramref name="reference"/> value are equal.
        /// </param>
        /// <returns>
        /// A new <see cref="Match{T}"/> instance that matches successfully when a value to match does not equal the specified <paramref name="reference"/> value.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="reference"/> is <c>null</c>. When matching an instance to be a non-<c>null</c> reference use <see cref="NotNull{T}"/> instead.</para>
        /// <para>- or -</para>
        /// <para><paramref name="comparer"/> is <c>null</c>.</para>
        /// </exception>
        public static Match<T> NotEqualTo<T>([NotNull] T reference, [NotNull] IComparer<T> comparer)
        {
            if (ReferenceEquals(reference, null))
            {
                throw new ArgumentNullException(nameof(reference));
            }

            if (ReferenceEquals(comparer, null))
            {
                throw new ArgumentNullException(nameof(comparer));
            }

            return Match<T>.Create(NotMatch<T>.Create(CompareMatch<T>.EqualTo(reference, comparer)));
        }

        /// <summary>
        /// Creates a <see cref="Match{T}"/> instance that matches successfully when a value to match is greater than the specified <paramref name="reference"/> value.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the value to match.
        /// </typeparam>
        /// <param name="reference">
        /// The instance a value to match must be greater than to match successfully.
        /// </param>
        /// <param name="comparer">
        /// The <see cref="IComparer{T}"/> instance to determine whether a value to match is greater than the specified <paramref name="reference"/> value.
        /// </param>
        /// <returns>
        /// A new <see cref="Match{T}"/> instance that matches successfully when a value to match is greater than the specified <paramref name="reference"/> value.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="reference"/> is <c>null</c>.</para>
        /// <para>- or -</para>
        /// <para><paramref name="comparer"/> is <c>null</c>.</para>
        /// </exception>
        public static Match<T> GreaterThan<T>([NotNull] T reference, [NotNull] IComparer<T> comparer)
        {
            if (ReferenceEquals(reference, null))
            {
                throw new ArgumentNullException(nameof(reference));
            }

            if (ReferenceEquals(comparer, null))
            {
                throw new ArgumentNullException(nameof(comparer));
            }

            return Match<T>.Create(CompareMatch<T>.GreaterThan(reference, comparer));
        }

        /// <summary>
        /// Creates a <see cref="Match{T}"/> instance that matches successfully when a value to match is greater than or equal to the specified <paramref name="reference"/> value.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the value to match.
        /// </typeparam>
        /// <param name="reference">
        /// The instance a value to match must be greater than or equal to match successfully.
        /// </param>
        /// <param name="comparer">
        /// The <see cref="IComparer{T}"/> instance to determine whether a value to match is greater than or equal to the specified <paramref name="reference"/> value.
        /// </param>
        /// <returns>
        /// A new <see cref="Match{T}"/> instance that matches successfully when a value to match is greater than or equal to the specified <paramref name="reference"/> value.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="reference"/> is <c>null</c>.</para>
        /// <para>- or -</para>
        /// <para><paramref name="comparer"/> is <c>null</c>.</para>
        /// </exception>
        public static Match<T> GreaterThanOrEqualTo<T>([NotNull] T reference, [NotNull] IComparer<T> comparer)
        {
            if (ReferenceEquals(reference, null))
            {
                throw new ArgumentNullException(nameof(reference));
            }

            if (ReferenceEquals(comparer, null))
            {
                throw new ArgumentNullException(nameof(comparer));
            }

            return Match<T>.Create(CompareMatch<T>.GreaterThanOrEqualTo(reference, comparer));
        }

        /// <summary>
        /// Creates a <see cref="Match{T}"/> instance that matches successfully when a value to match is less than the specified <paramref name="reference"/> value.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the value to match.
        /// </typeparam>
        /// <param name="reference">
        /// The instance a value to match must be less than to match successfully.
        /// </param>
        /// <param name="comparer">
        /// The <see cref="IComparer{T}"/> instance to determine whether a value to match is less than the specified <paramref name="reference"/> value.
        /// </param>
        /// <returns>
        /// A new <see cref="Match{T}"/> instance that matches successfully when a value to match is less than the specified <paramref name="reference"/> value.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="reference"/> is <c>null</c>.</para>
        /// <para>- or -</para>
        /// <para><paramref name="comparer"/> is <c>null</c>.</para>
        /// </exception>
        public static Match<T> LessThan<T>([NotNull] T reference, [NotNull] IComparer<T> comparer)
        {
            if (ReferenceEquals(reference, null))
            {
                throw new ArgumentNullException(nameof(reference));
            }

            if (ReferenceEquals(comparer, null))
            {
                throw new ArgumentNullException(nameof(comparer));
            }

            return Match<T>.Create(CompareMatch<T>.LessThan(reference, comparer));
        }

        /// <summary>
        /// Creates a <see cref="Match{T}"/> instance that matches successfully when a value to match is less than or equal to the specified <paramref name="reference"/> value.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the value to match.
        /// </typeparam>
        /// <param name="reference">
        /// The instance a value to match must be less than or equal to match successfully.
        /// </param>
        /// <param name="comparer">
        /// The <see cref="IComparer{T}"/> instance to determine whether a value to match is less than or equal to the specified <paramref name="reference"/> value.
        /// </param>
        /// <returns>
        /// A new <see cref="Match{T}"/> instance that matches successfully when a value to match is less than or equal to the specified <paramref name="reference"/> value.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="reference"/> is <c>null</c>.</para>
        /// <para>- or -</para>
        /// <para><paramref name="comparer"/> is <c>null</c>.</para>
        /// </exception>
        public static Match<T> LessThanOrEqualTo<T>([NotNull] T reference, [NotNull] IComparer<T> comparer)
        {
            if (ReferenceEquals(reference, null))
            {
                throw new ArgumentNullException(nameof(reference));
            }

            if (ReferenceEquals(comparer, null))
            {
                throw new ArgumentNullException(nameof(comparer));
            }

            return Match<T>.Create(CompareMatch<T>.LessThanOrEqualTo(reference, comparer));
        }

        private sealed class ComparisonMatch<T> : IMatch<T>
        {
            private delegate bool MatchDelegate(int comparisonResult);

            private readonly IComparison<T> comparison;

            private readonly MatchDelegate match;

            private ComparisonMatch(IComparison<T> comparison, MatchDelegate match)
            {
                this.comparison = comparison;
                this.match = match;
            }

            public static ComparisonMatch<T> EqualTo(IComparison<T> comparison)
                => new ComparisonMatch<T>(comparison, EqualToMatch);

            private static bool EqualToMatch(int comparisonResult)
                => comparisonResult == 0;

            public bool Matches(T value)
                => this.match(this.comparison.Compare(value));
        }

        private sealed class CompareMatch<T> : IMatch<T>
        {
            private delegate bool Comparison(T left, T right, IComparer<T> comparer);

            private readonly T reference;

            private readonly IComparer<T> comparer;

            private readonly Comparison comparison;

            private CompareMatch(T reference, IComparer<T> comparer, Comparison comparison)
            {
                this.reference = reference;
                this.comparer = comparer;
                this.comparison = comparison;
            }

            public static CompareMatch<T> EqualTo(T reference, IComparer<T> comparer)
                => new CompareMatch<T>(reference, comparer, EqualToComparison);

            private static bool EqualToComparison(T left, T right, IComparer<T> comparer)
                => comparer.Compare(left, right) == 0;

            public static CompareMatch<T> GreaterThan(T reference, IComparer<T> comparer)
                => new CompareMatch<T>(reference, comparer, GreaterThanComparison);

            private static bool GreaterThanComparison(T left, T right, IComparer<T> comparer)
                => comparer.Compare(left, right) > 0;

            public static CompareMatch<T> GreaterThanOrEqualTo(T reference, IComparer<T> comparer)
                => new CompareMatch<T>(reference, comparer, GreaterThanOrEqualToComparison);

            private static bool GreaterThanOrEqualToComparison(T left, T right, IComparer<T> comparer)
                => comparer.Compare(left, right) >= 0;

            public static CompareMatch<T> LessThan(T reference, IComparer<T> comparer)
                => new CompareMatch<T>(reference, comparer, LessThanComparison);

            private static bool LessThanComparison<T>(T left, T right, IComparer<T> comparer)
                => comparer.Compare(left, right) < 0;

            public static CompareMatch<T> LessThanOrEqualTo(T reference, IComparer<T> comparer)
                => new CompareMatch<T>(reference, comparer, LessThanOrEqualToComparison);

            private static bool LessThanOrEqualToComparison(T left, T right, IComparer<T> comparer)
                => comparer.Compare(left, right) <= 0;

            public bool Matches(T value)
                => this.comparison(value, this.reference, this.comparer);
        }

        /// <summary>
        /// Creates a <see cref="Match{T}"/> instance that matches successfully when a value matches all of the specified <paramref name="matches"/>.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the value to match.
        /// </typeparam>
        /// <param name="matches">
        /// The matches a value must match all to match successfully.
        /// </param>
        /// <returns>
        /// A new <see cref="Match{T}"/> instance that matches successfully when a value matches all of the specified <paramref name="matches"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="matches"/> is <c>null</c>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="matches"/> contain at least one match that is <c>null</c>.
        /// </exception>
        public static Match<T> All<T>(params Match<T>[] matches)
        {
            if (ReferenceEquals(matches, null!))
            {
                throw new ArgumentNullException(nameof(matches));
            }

            if (matches.Any(match => ReferenceEquals(match, null)))
            {
                throw new ArgumentException("At least one match is a null reference.", nameof(matches));
            }

            return Match<T>.All(matches);
        }

        internal static IMatch<T> All<T>(params IMatch<T>[] matches)
            => CompositeMatch<T>.All(matches);

        /// <summary>
        /// Creates a <see cref="Match{T}"/> instance that matches successfully when a value matches any of the specified <paramref name="matches"/>.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the value to match.
        /// </typeparam>
        /// <param name="matches">
        /// The matches a value must match any to match successfully.
        /// </param>
        /// <returns>
        /// A new <see cref="Match{T}"/> instance that matches successfully when a value matches any of the specified <paramref name="matches"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="matches"/> is <c>null</c>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="matches"/> contain at least one match that is <c>null</c>.
        /// </exception>
        public static Match<T> Any<T>(params Match<T>[] matches)
        {
            if (ReferenceEquals(matches, null!))
            {
                throw new ArgumentNullException(nameof(matches));
            }

            if (matches.Any(match => ReferenceEquals(match, null)))
            {
                throw new ArgumentException("At least one match is a null reference.", nameof(matches));
            }

            return Match<T>.Any(matches);
        }

        internal static IMatch<T> Any<T>(params IMatch<T>[] matches)
            => CompositeMatch<T>.Any(matches);

        /// <summary>
        /// Creates a <see cref="Match{T}"/> instance that matches successfully when a value matches none of the specified <paramref name="matches"/>.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the value to match.
        /// </typeparam>
        /// <param name="matches">
        /// The matches a value must match none to match successfully.
        /// </param>
        /// <returns>
        /// A new <see cref="Match{T}"/> instance that matches successfully when a value matches none of the specified <paramref name="matches"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="matches"/> is <c>null</c>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="matches"/> contain at least one match that is <c>null</c>.
        /// </exception>
        public static Match<T> None<T>(params Match<T>[] matches)
        {
            if (ReferenceEquals(matches, null!))
            {
                throw new ArgumentNullException(nameof(matches));
            }

            if (matches.Any(match => ReferenceEquals(match, null)))
            {
                throw new ArgumentException("At least one match is a null reference.", nameof(matches));
            }

            return Match<T>.None(matches);
        }

        internal static IMatch<T> None<T>(params IMatch<T>[] matches)
            => NotMatch<T>.Create(CompositeMatch<T>.Any(matches));

        /// <summary>
        /// Creates a <see cref="Match{T}"/> instance that matches successfully when a value matches none of the specified <paramref name="matches"/>.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the value to match.
        /// </typeparam>
        /// <param name="matches">
        /// The matches a value must match none to match successfully.
        /// </param>
        /// <returns>
        /// A new <see cref="Match{T}"/> instance that matches successfully when a value matches none of the specified <paramref name="matches"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="matches"/> is <c>null</c>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="matches"/> contain at least one match that is <c>null</c>.
        /// </exception>
        [Obsolete("This method only exists for compatibility reasons and has been replaced by 'None' method due to better naming. It will be removed in an upcoming release.")]
        public static Match<T> Except<T>(params Match<T>[] matches)
            => None(matches);

        private sealed class CompositeMatch<T> : IMatch<T>
        {
            private delegate bool CompositeMatchDelegate(IEnumerable<IMatch<T>> matches, T value);

            private readonly IMatch<T>[] matches;

            private readonly CompositeMatchDelegate compositeMatch;

            private CompositeMatch(IMatch<T>[] matches, CompositeMatchDelegate compositeMatch)
            {
                this.matches = matches;
                this.compositeMatch = compositeMatch;
            }

            public static CompositeMatch<T> All(IMatch<T>[] matches)
                => new CompositeMatch<T>(matches, MatchAll);

            private static bool MatchAll(IEnumerable<IMatch<T>> matches, T value)
                => matches.All(match => match.Matches(value));

            public static CompositeMatch<T> Any(IMatch<T>[] matches)
                => new CompositeMatch<T>(matches, MatchAny);

            private static bool MatchAny(IEnumerable<IMatch<T>> matches, T value)
                => matches.Any(match => match.Matches(value));

            public bool Matches(T value)
                => this.compositeMatch(this.matches, value);
        }

        /// <summary>
        /// Creates a <see cref="Match{T}"/> instance that matches successfully when a value matches according to the given <paramref name="customMatch"/>.
        /// </summary>
        /// <typeparam name="TValue">
        /// The type of the value to match.
        /// </typeparam>
        /// <typeparam name="TCustomMatch">
        /// The type of the custom match that must implement the <see cref="ICustomMatch{T}"/> interface.
        /// </typeparam>
        /// <param name="customMatch">
        /// The custom match a value is matched with.
        /// </param>
        /// <returns>
        /// A new <see cref="Match{T}"/> instance that matches successfully when a value matches according to the given <paramref name="customMatch"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="customMatch"/> is <c>null</c>.
        /// </exception>
        public static Match<TValue> Custom<TValue, TCustomMatch>([NotNull] TCustomMatch customMatch)
            where TCustomMatch : ICustomMatch<TValue>
        {
            if (ReferenceEquals(customMatch, null))
            {
                throw new ArgumentNullException(nameof(customMatch));
            }

            return Match<TValue>.Create(CustomMatch<TValue, TCustomMatch>.Create(customMatch));
        }

        private sealed class CustomMatch<TValue, TCustomMatch> : IMatch<TValue>
            where TCustomMatch : ICustomMatch<TValue>
        {
            private readonly TCustomMatch customMatch;

            private CustomMatch(TCustomMatch customMatch)
            {
                this.customMatch = customMatch;
            }

            public static CustomMatch<TValue, TCustomMatch> Create(TCustomMatch customMatch)
                => new CustomMatch<TValue, TCustomMatch>(customMatch);

            public bool Matches(TValue value)
                => this.customMatch.Matches(value);
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
    public sealed class Match<T>
    {
        private readonly IMatch<T> match;

        private Match(IMatch<T> match)
        {
            this.match = match;
        }

        internal static Match<T> Create(IMatch<T> match)
            => new Match<T>(match);

        internal static Match<T> All(IEnumerable<Match<T>> matches)
            => Create(Match.All(matches.Select(item => item.match).ToArray()));

        internal static Match<T> Any(IEnumerable<Match<T>> matches)
            => Create(Match.Any(matches.Select(item => item.match).ToArray()));

        internal static Match<T> None(IEnumerable<Match<T>> matches)
            => Create(Match.None(matches.Select(item => item.match).ToArray()));

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

    /// <summary>
    /// Represents a strongly typed custom match that provides a method to determine whether a value matches with.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the value to match.
    /// </typeparam>
    public interface ICustomMatch<in T>
    {
        /// <summary>
        /// Determines whether the specified <paramref name="value"/> successfully matches according to this match.
        /// </summary>
        /// <param name="value">
        /// The value to match.
        /// </param>
        /// <returns>
        /// <c>true</c> if <paramref name="value"/> successfully matches according to this match; otherwise, <c>false</c>.
        /// </returns>
        bool Matches(T value);
    }

    internal interface IMatch<in T>
    {
        bool Matches(T value);
    }
}
