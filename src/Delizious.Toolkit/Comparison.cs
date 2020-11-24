namespace Delizious
{
    using System;

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
    }

    internal interface IComparison<in T>
    {
        int Compare(T value);
    }
}