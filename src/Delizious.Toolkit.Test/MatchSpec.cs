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

        private static object[] DataTheory(params object[] values)
            => values;
    }
}
