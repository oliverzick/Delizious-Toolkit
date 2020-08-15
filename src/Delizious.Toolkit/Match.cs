namespace Delizious
{
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
