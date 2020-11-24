namespace Delizious
{
    using System;
    using System.Collections.Generic;

    internal static class Comparison
    {
        public static IComparison<T> Comparable<T>(T reference)
            where T : IComparable<T>
            => ComparableComparison<T>.Create(reference);

        private sealed class ComparableComparison<T> : IComparison<T>
            where T : IComparable<T>
        {
            private readonly T reference;

            private ComparableComparison(T reference)
            {
                this.reference = reference;
            }

            public static ComparableComparison<T> Create(T reference)
                => new ComparableComparison<T>(reference);

            public int Compare(T value)
                => value.CompareTo(this.reference);
        }

        public static IComparison<T> Comparer<T>(T reference, IComparer<T> comparer)
            => ComparerComparison<T>.Create(reference, comparer);

        private sealed class ComparerComparison<T> : IComparison<T>
        {
            private readonly T reference;

            private readonly IComparer<T> comparer;

            private ComparerComparison(T reference, IComparer<T> comparer)
            {
                this.reference = reference;
                this.comparer = comparer;
            }

            public static ComparerComparison<T> Create(T reference, IComparer<T> comparer)
                => new ComparerComparison<T>(reference, comparer);

            public int Compare(T value)
                => this.comparer.Compare(value, this.reference);
        }
    }

    internal interface IComparison<in T>
    {
        int Compare(T value);
    }
}
