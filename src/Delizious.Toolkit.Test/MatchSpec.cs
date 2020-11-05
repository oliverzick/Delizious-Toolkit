namespace Delizious
{
    using System;
    using System.Collections.Generic;
    using Xunit;

    public sealed class MatchSpec
    {
        public sealed class Always
        {
            [Theory]
            [InlineData(true, false)]
            [InlineData(true, true)]
            public void Matches(bool expected, bool value)
            {
                var subject = Match.Always<bool>();

                var actual = subject.Matches(value);

                Assert.Equal(expected, actual);
            }
        }

        public sealed class Never
        {
            [Theory]
            [InlineData(false, false)]
            [InlineData(false, true)]
            public void Matches(bool expected, bool value)
            {
                var subject = Match.Never<bool>();

                var actual = subject.Matches(value);

                Assert.Equal(expected, actual);
            }
        }

        public sealed class Null
        {
            [Theory]
            [InlineData(true, null)]
            [InlineData(false, "")]
            [InlineData(false, "Sample string")]
            public void Matches(bool expected, string value)
            {
                var subject = Match.Null<string>();

                var actual = subject.Matches(value);

                Assert.Equal(expected, actual);
            }
        }

        public sealed class NotNull
        {
            [Theory]
            [InlineData(false, null)]
            [InlineData(true, "")]
            [InlineData(true, "Sample string")]
            public void Matches(bool expected, string value)
            {
                var subject = Match.NotNull<string>();

                var actual = subject.Matches(value);

                Assert.Equal(expected, actual);
            }
        }

        public sealed class Equal
        {
            [Fact]
            public void Throws_exception_when_reference_value_is_null()
            {
                Assert.Throws<ArgumentNullException>(() => Match.Equal(null, EqualityComparer<string>.Default));
            }

            [Fact]
            public void Throws_exception_when_equality_comparer_is_null()
            {
                Assert.Throws<ArgumentNullException>(() => Match.Equal(string.Empty, null!));
            }

            [Theory]
            [MemberData(nameof(MatchesTheories))]
            public void Matches(bool expected, string reference, string value, IEqualityComparer<string> equalityComparer)
            {
                var subject = Match.Equal(reference, equalityComparer);

                var actual = subject.Matches(value);

                Assert.Equal(expected, actual);
            }

            public static IEnumerable<object[]> MatchesTheories()
            {
                yield return DataTheory(true, "", "", StringComparer.Ordinal);
                yield return DataTheory(true, "Test", "Test", StringComparer.Ordinal);
                yield return DataTheory(false, "Test", "Tes", StringComparer.Ordinal);
                yield return DataTheory(false, "Test", "Testt", StringComparer.Ordinal);
                yield return DataTheory(false, "Test", "test", StringComparer.Ordinal);
                yield return DataTheory(false, "Test", "TEST", StringComparer.Ordinal);
                yield return DataTheory(true, "Test", "TEST", StringComparer.OrdinalIgnoreCase);
                yield return DataTheory(true, "Test", "test", StringComparer.OrdinalIgnoreCase);
            }
        }

        public sealed class NotEqual
        {
            [Fact]
            public void Throws_exception_when_reference_value_is_null()
            {
                Assert.Throws<ArgumentNullException>(() => Match.NotEqual(null, EqualityComparer<string>.Default));
            }

            [Fact]
            public void Throws_exception_when_equality_comparer_is_null()
            {
                Assert.Throws<ArgumentNullException>(() => Match.NotEqual(string.Empty, null!));
            }

            [Theory]
            [MemberData(nameof(MatchesTheories))]
            public void Matches(bool expected, string reference, string value, IEqualityComparer<string> equalityComparer)
            {
                var subject = Match.NotEqual(reference, equalityComparer);

                var actual = subject.Matches(value);

                Assert.Equal(expected, actual);
            }

            public static IEnumerable<object[]> MatchesTheories()
            {
                yield return DataTheory(false, "", "", StringComparer.Ordinal);
                yield return DataTheory(false, "Test", "Test", StringComparer.Ordinal);
                yield return DataTheory(true, "Test", "Tes", StringComparer.Ordinal);
                yield return DataTheory(true, "Test", "Testt", StringComparer.Ordinal);
                yield return DataTheory(true, "Test", "test", StringComparer.Ordinal);
                yield return DataTheory(true, "Test", "TEST", StringComparer.Ordinal);
                yield return DataTheory(false, "Test", "TEST", StringComparer.OrdinalIgnoreCase);
                yield return DataTheory(false, "Test", "test", StringComparer.OrdinalIgnoreCase);
            }
        }

        public sealed class Same
        {
            [Fact]
            public void Throws_exception_when_reference_value_is_null()
            {
                Assert.Throws<ArgumentNullException>(() => Match.Same<object>(null!));
            }

            [Theory]
            [MemberData(nameof(MatchesTheories))]
            public void Matches(bool expected, object reference, object value)
            {
                var subject = Match.Same(reference);

                var actual = subject.Matches(value);

                Assert.Equal(expected, actual);
            }

            public static IEnumerable<object[]> MatchesTheories()
            {
                var instance1 = new object();
                var instance2 = new object();

                yield return DataTheory(true, instance1, instance1);
                yield return DataTheory(true, instance2, instance2);
                yield return DataTheory(false, instance1, instance2);
            }
        }

        public sealed class NotSame
        {
            [Fact]
            public void Throws_exception_when_reference_value_is_null()
            {
                Assert.Throws<ArgumentNullException>(() => Match.NotSame<object>(null!));
            }

            [Theory]
            [MemberData(nameof(MatchesTheories))]
            public void Matches(bool expected, object reference, object value)
            {
                var subject = Match.NotSame(reference);

                var actual = subject.Matches(value);

                Assert.Equal(expected, actual);
            }

            public static IEnumerable<object[]> MatchesTheories()
            {
                var instance1 = new object();
                var instance2 = new object();

                yield return DataTheory(false, instance1, instance1);
                yield return DataTheory(false, instance2, instance2);
                yield return DataTheory(true, instance1, instance2);
            }
        }

        public sealed class GreaterThan
        {
            [Fact]
            public void Throws_exception_when_reference_value_is_null()
            {
                Assert.Throws<ArgumentNullException>(() => Match.GreaterThan<string>(null!, Comparer<string>.Default));
            }

            [Fact]
            public void Throws_exception_when_comparer_is_null()
            {
                Assert.Throws<ArgumentNullException>(() => Match.GreaterThan<string>(string.Empty, null!));
            }

            [Theory]
            [MemberData(nameof(MatchesTheories))]
            public void Matches(bool expected, string value, string reference, IComparer<string> comparer)
            {
                var subject = Match.GreaterThan(reference, comparer);

                var actual = subject.Matches(value);

                Assert.Equal(expected, actual);
            }

            public static IEnumerable<object[]> MatchesTheories()
            {
                yield return DataTheory(false, "A", "B", StringComparer.Ordinal);
                yield return DataTheory(true, "B", "A", StringComparer.Ordinal);
                yield return DataTheory(false, "A", "A", StringComparer.Ordinal);
            }
        }

        public sealed class GreaterThanOrEqualTo
        {
            [Fact]
            public void Throws_exception_when_reference_value_is_null()
            {
                Assert.Throws<ArgumentNullException>(() => Match.GreaterThanOrEqualTo(null!, Comparer<string>.Default));
            }

            [Fact]
            public void Throws_exception_when_comparer_is_null()
            {
                Assert.Throws<ArgumentNullException>(() => Match.GreaterThanOrEqualTo(string.Empty, null!));
            }

            [Theory]
            [MemberData(nameof(MatchesTheories))]
            public void Matches(bool expected, string value, string reference, IComparer<string> comparer)
            {
                var subject = Match.GreaterThanOrEqualTo(reference, comparer);

                var actual = subject.Matches(value);

                Assert.Equal(expected, actual);
            }

            public static IEnumerable<object[]> MatchesTheories()
            {
                yield return DataTheory(false, "A", "B", StringComparer.Ordinal);
                yield return DataTheory(true, "B", "A", StringComparer.Ordinal);
                yield return DataTheory(true, "A", "A", StringComparer.Ordinal);
            }
        }

        private static object[] DataTheory(params object[] values)
            => values;
    }
}
