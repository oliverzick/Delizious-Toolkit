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
            [InlineData(true, null)]
            [InlineData(true, "")]
            [InlineData(true, "A")]
            public void Matches(bool expected, string value)
            {
                var subject = Match.Always<string>();

                var actual = subject.Matches(value);

                Assert.Equal(expected, actual);
            }
        }

        public sealed class Never
        {
            [Theory]
            [InlineData(false, null)]
            [InlineData(false, "")]
            [InlineData(false, "A")]
            public void Matches(bool expected, string value)
            {
                var subject = Match.Never<string>();

                var actual = subject.Matches(value);

                Assert.Equal(expected, actual);
            }
        }

        public sealed class Null
        {
            [Theory]
            [InlineData(true,  null)]
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
            [InlineData(true,  "")]
            [InlineData(true,  "Sample string")]
            public void Matches(bool expected, string value)
            {
                var subject = Match.NotNull<string>();

                var actual = subject.Matches(value);

                Assert.Equal(expected, actual);
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
            public void Matches(bool expected, object value, object reference)
            {
                var subject = Match.Same(reference);

                var actual = subject.Matches(value);

                Assert.Equal(expected, actual);
            }

            public static IEnumerable<object[]> MatchesTheories()
            {
                var instance1 = new object();
                var instance2 = new object();

                yield return DataTheory(false, null,      instance1);
                yield return DataTheory(false, null,      instance2);
                yield return DataTheory(true,  instance1, instance1);
                yield return DataTheory(true,  instance2, instance2);
                yield return DataTheory(false, instance1, instance2);
                yield return DataTheory(false, instance2, instance1);
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
            public void Matches(bool expected, object value, object reference)
            {
                var subject = Match.NotSame(reference);

                var actual = subject.Matches(value);

                Assert.Equal(expected, actual);
            }

            public static IEnumerable<object[]> MatchesTheories()
            {
                var instance1 = new object();
                var instance2 = new object();

                yield return DataTheory(true,  null,      instance1);
                yield return DataTheory(true,  null,      instance2);
                yield return DataTheory(false, instance1, instance1);
                yield return DataTheory(false, instance2, instance2);
                yield return DataTheory(true,  instance1, instance2);
                yield return DataTheory(true,  instance2, instance1);
            }
        }

        public sealed class Equal_Equatable
        {
            [Fact]
            public void Throws_exception_when_reference_value_is_null()
            {
                Assert.Throws<ArgumentNullException>(() => Match.Equal<string>(null));
            }

            [Theory]
            [MemberData(nameof(MatchesTheories))]
            public void Matches(bool expected, string value, string reference)
            {
                var subject = Match.Equal(reference);

                var actual = subject.Matches(value);

                Assert.Equal(expected, actual);
            }

            public static IEnumerable<object[]> MatchesTheories()
            {
                yield return DataTheory(false, null,         string.Empty);
                yield return DataTheory(false, null,         "A");
                yield return DataTheory(true,  string.Empty, string.Empty);
                yield return DataTheory(false, string.Empty, "A");
                yield return DataTheory(true,  "A",          "A");
                yield return DataTheory(false, "A",          "B");
                yield return DataTheory(false, "B",          "A");
                yield return DataTheory(true,  "B",          "B");
            }
        }

        public sealed class NotEqual_Equatable
        {
            [Fact]
            public void Throws_exception_when_reference_value_is_null()
            {
                Assert.Throws<ArgumentNullException>(() => Match.NotEqual<string>(null));
            }

            [Theory]
            [MemberData(nameof(MatchesTheories))]
            public void Matches(bool expected, string value, string reference)
            {
                var subject = Match.NotEqual(reference);

                var actual = subject.Matches(value);

                Assert.Equal(expected, actual);
            }

            public static IEnumerable<object[]> MatchesTheories()
            {
                yield return DataTheory(true,  null,         string.Empty);
                yield return DataTheory(true,  null,         "A");
                yield return DataTheory(false, string.Empty, string.Empty);
                yield return DataTheory(true,  string.Empty, "A");
                yield return DataTheory(false, "A",          "A");
                yield return DataTheory(true,  "A",          "B");
                yield return DataTheory(true,  "B",          "A");
                yield return DataTheory(false, "B",          "B");
            }
        }

        public sealed class Equal_EqualityComparer
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
            public void Matches(bool expected, string value, string reference, IEqualityComparer<string> equalityComparer)
            {
                var subject = Match.Equal(reference, equalityComparer);

                var actual = subject.Matches(value);

                Assert.Equal(expected, actual);
            }

            public static IEnumerable<object[]> MatchesTheories()
            {
                yield return DataTheory(false, null,         string.Empty, StringComparer.Ordinal);
                yield return DataTheory(false, null,         "A",          StringComparer.Ordinal);
                yield return DataTheory(true,  string.Empty, string.Empty, StringComparer.Ordinal);
                yield return DataTheory(false, string.Empty, "A",          StringComparer.Ordinal);
                yield return DataTheory(true,  "Test",       "Test",       StringComparer.Ordinal);
                yield return DataTheory(false, "Tes",        "Test",       StringComparer.Ordinal);
                yield return DataTheory(false, "Testt",      "Test",       StringComparer.Ordinal);
                yield return DataTheory(false, "test",       "Test",       StringComparer.Ordinal);
                yield return DataTheory(false, "TEST",       "Test",       StringComparer.Ordinal);
                yield return DataTheory(true,  "TEST",       "Test",       StringComparer.OrdinalIgnoreCase);
                yield return DataTheory(true,  "test",       "Test",       StringComparer.OrdinalIgnoreCase);
            }
        }

        public sealed class NotEqual_EqualityComparer
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
            public void Matches(bool expected, string value, string reference, IEqualityComparer<string> equalityComparer)
            {
                var subject = Match.NotEqual(reference, equalityComparer);

                var actual = subject.Matches(value);

                Assert.Equal(expected, actual);
            }

            public static IEnumerable<object[]> MatchesTheories()
            {
                yield return DataTheory(true,  null,         string.Empty, StringComparer.Ordinal);
                yield return DataTheory(true,  null,         "A",          StringComparer.Ordinal);
                yield return DataTheory(false, string.Empty, string.Empty, StringComparer.Ordinal);
                yield return DataTheory(true,  string.Empty, "A",          StringComparer.Ordinal);
                yield return DataTheory(false, "Test",       "Test",       StringComparer.Ordinal);
                yield return DataTheory(true,  "Tes",        "Test",       StringComparer.Ordinal);
                yield return DataTheory(true,  "Testt",      "Test",       StringComparer.Ordinal);
                yield return DataTheory(true,  "test",       "Test",       StringComparer.Ordinal);
                yield return DataTheory(true,  "TEST",       "Test",       StringComparer.Ordinal);
                yield return DataTheory(false, "TEST",       "Test",       StringComparer.OrdinalIgnoreCase);
                yield return DataTheory(false, "test",       "Test",       StringComparer.OrdinalIgnoreCase);
            }
        }

        public sealed class EqualTo_Comparable
        {
            [Fact]
            public void Throws_exception_when_reference_value_is_null()
            {
                Assert.Throws<ArgumentNullException>(() => Match.EqualTo<string>(null));
            }

            [Theory]
            [MemberData(nameof(MatchesTheories))]
            public void Matches(bool expected, string value, string reference)
            {
                var subject = Match.EqualTo(reference);

                var actual = subject.Matches(value);

                Assert.Equal(expected, actual);
            }

            public static IEnumerable<object[]> MatchesTheories()
            {
                yield return DataTheory(false, null,         "A");
                yield return DataTheory(false, string.Empty, "A");
                yield return DataTheory(true,  "A",          "A");
                yield return DataTheory(false, "A",          "B");
                yield return DataTheory(false, "B",          "A");
                yield return DataTheory(true,  "B",          "B");
            }
        }

        public sealed class EqualTo_Comparer
        {
            [Fact]
            public void Throws_exception_when_reference_value_is_null()
            {
                Assert.Throws<ArgumentNullException>(() => Match.EqualTo(null, Comparer<string>.Default));
            }

            [Fact]
            public void Throws_exception_when_comparer_is_null()
            {
                Assert.Throws<ArgumentNullException>(() => Match.EqualTo(string.Empty, null!));
            }

            [Theory]
            [MemberData(nameof(MatchesTheories))]
            public void Matches(bool expected, string value, string reference, IComparer<string> comparer)
            {
                var subject = Match.EqualTo(reference, comparer);

                var actual = subject.Matches(value);

                Assert.Equal(expected, actual);
            }

            public static IEnumerable<object[]> MatchesTheories()
            {
                yield return DataTheory(false, null,         "A", StringComparer.Ordinal);
                yield return DataTheory(false, string.Empty, "A", StringComparer.Ordinal);
                yield return DataTheory(true,  "A",          "A", StringComparer.Ordinal);
                yield return DataTheory(false, "A",          "B", StringComparer.Ordinal);
                yield return DataTheory(false, "B",          "A", StringComparer.Ordinal);
                yield return DataTheory(true,  "B",          "B", StringComparer.Ordinal);
                yield return DataTheory(false, "b",          "A", StringComparer.OrdinalIgnoreCase);
                yield return DataTheory(false, "a",          "B", StringComparer.OrdinalIgnoreCase);
                yield return DataTheory(true,  "b",          "B", StringComparer.OrdinalIgnoreCase);
            }
        }

        public sealed class NotEqualTo_Comparable
        {
            [Fact]
            public void Throws_exception_when_reference_value_is_null()
            {
                Assert.Throws<ArgumentNullException>(() => Match.NotEqualTo<string>(null));
            }

            [Theory]
            [MemberData(nameof(MatchesTheories))]
            public void Matches(bool expected, string value, string reference)
            {
                var subject = Match.NotEqualTo(reference);

                var actual = subject.Matches(value);

                Assert.Equal(expected, actual);
            }

            public static IEnumerable<object[]> MatchesTheories()
            {
                yield return DataTheory(true,  null,         "A");
                yield return DataTheory(true,  string.Empty, "A");
                yield return DataTheory(false, "A",          "A");
                yield return DataTheory(true,  "A",          "B");
                yield return DataTheory(true,  "B",          "A");
                yield return DataTheory(false, "B",          "B");
            }
        }

        public sealed class NotEqualTo_Comparer
        {
            [Fact]
            public void Throws_exception_when_reference_value_is_null()
            {
                Assert.Throws<ArgumentNullException>(() => Match.NotEqualTo(null, Comparer<string>.Default));
            }

            [Fact]
            public void Throws_exception_when_comparer_is_null()
            {
                Assert.Throws<ArgumentNullException>(() => Match.NotEqualTo(string.Empty, null!));
            }

            [Theory]
            [MemberData(nameof(MatchesTheories))]
            public void Matches(bool expected, string value, string reference, IComparer<string> comparer)
            {
                var subject = Match.NotEqualTo(reference, comparer);

                var actual = subject.Matches(value);

                Assert.Equal(expected, actual);
            }

            public static IEnumerable<object[]> MatchesTheories()
            {
                yield return DataTheory(true,  null,         "A", StringComparer.Ordinal);
                yield return DataTheory(true,  string.Empty, "A", StringComparer.Ordinal);
                yield return DataTheory(false, "A",          "A", StringComparer.Ordinal);
                yield return DataTheory(true,  "A",          "B", StringComparer.Ordinal);
                yield return DataTheory(true,  "B",          "A", StringComparer.Ordinal);
                yield return DataTheory(false, "B",          "B", StringComparer.Ordinal);
                yield return DataTheory(true,  "b",          "A", StringComparer.OrdinalIgnoreCase);
                yield return DataTheory(true,  "a",          "B", StringComparer.OrdinalIgnoreCase);
                yield return DataTheory(false, "b",          "B", StringComparer.OrdinalIgnoreCase);
            }
        }

        public sealed class GreaterThan_Comparable
        {
            [Fact]
            public void Throws_exception_when_reference_value_is_null()
            {
                Assert.Throws<ArgumentNullException>(() => Match.GreaterThan<string>(null));
            }

            [Theory]
            [MemberData(nameof(MatchesTheories))]
            public void Matches(bool expected, string value, string reference)
            {
                var subject = Match.GreaterThan(reference);

                var actual = subject.Matches(value);

                Assert.Equal(expected, actual);
            }

            public static IEnumerable<object[]> MatchesTheories()
            {
                yield return DataTheory(false, null,         "A");
                yield return DataTheory(false, string.Empty, "A");
                yield return DataTheory(false, "A",          "A");
                yield return DataTheory(false, "A",          "B");
                yield return DataTheory(true,  "B",          "A");
                yield return DataTheory(false, "B",          "B");
            }
        }

        public sealed class GreaterThan_Comparer
        {
            [Fact]
            public void Throws_exception_when_reference_value_is_null()
            {
                Assert.Throws<ArgumentNullException>(() => Match.GreaterThan(null!, Comparer<string>.Default));
            }

            [Fact]
            public void Throws_exception_when_comparer_is_null()
            {
                Assert.Throws<ArgumentNullException>(() => Match.GreaterThan(string.Empty, null!));
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
                yield return DataTheory(false, null,         "A", StringComparer.Ordinal);
                yield return DataTheory(false, string.Empty, "A", StringComparer.Ordinal);
                yield return DataTheory(false, "A",          "A", StringComparer.Ordinal);
                yield return DataTheory(false, "A",          "B", StringComparer.Ordinal);
                yield return DataTheory(true,  "B",          "A", StringComparer.Ordinal);
                yield return DataTheory(false, "B",          "B", StringComparer.Ordinal);
                yield return DataTheory(true,  "b",          "A", StringComparer.OrdinalIgnoreCase);
                yield return DataTheory(false, "a",          "B", StringComparer.OrdinalIgnoreCase);
                yield return DataTheory(false, "b",          "B", StringComparer.OrdinalIgnoreCase);
            }
        }

        public sealed class GreaterThanOrEqualTo_Comparable
        {
            [Fact]
            public void Throws_exception_when_reference_value_is_null()
            {
                Assert.Throws<ArgumentNullException>(() => Match.GreaterThanOrEqualTo<string>(null));
            }

            [Theory]
            [MemberData(nameof(MatchesTheories))]
            public void Matches(bool expected, string value, string reference)
            {
                var subject = Match.GreaterThanOrEqualTo(reference);

                var actual = subject.Matches(value);

                Assert.Equal(expected, actual);
            }

            public static IEnumerable<object[]> MatchesTheories()
            {
                yield return DataTheory(false, null,         "A");
                yield return DataTheory(false, string.Empty, "A");
                yield return DataTheory(true,  "A",          "A");
                yield return DataTheory(false, "A",          "B");
                yield return DataTheory(true,  "B",          "A");
                yield return DataTheory(true,  "B",          "B");
            }
        }

        public sealed class GreaterThanOrEqualTo_Comparer
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
                yield return DataTheory(false, null,         "A", StringComparer.Ordinal);
                yield return DataTheory(false, string.Empty, "A", StringComparer.Ordinal);
                yield return DataTheory(true,  "A",          "A", StringComparer.Ordinal);
                yield return DataTheory(false, "A",          "B", StringComparer.Ordinal);
                yield return DataTheory(true,  "B",          "A", StringComparer.Ordinal);
                yield return DataTheory(true,  "B",          "B", StringComparer.Ordinal);
                yield return DataTheory(true,  "b",          "A", StringComparer.OrdinalIgnoreCase);
                yield return DataTheory(false, "a",          "B", StringComparer.OrdinalIgnoreCase);
                yield return DataTheory(true,  "b",          "B", StringComparer.OrdinalIgnoreCase);
            }
        }

        public sealed class LessThan_Comparable
        {
            [Fact]
            public void Throws_exception_when_reference_value_is_null()
            {
                Assert.Throws<ArgumentNullException>(() => Match.LessThan<string>(null));
            }

            [Theory]
            [MemberData(nameof(MatchesTheories))]
            public void Matches(bool expected, string value, string reference)
            {
                var subject = Match.LessThan(reference);

                var actual = subject.Matches(value);

                Assert.Equal(expected, actual);
            }

            public static IEnumerable<object[]> MatchesTheories()
            {
                yield return DataTheory(true,  null,         "A");
                yield return DataTheory(true,  string.Empty, "A");
                yield return DataTheory(false, "A",          "A");
                yield return DataTheory(true,  "A",          "B");
                yield return DataTheory(false, "B",          "A");
                yield return DataTheory(false, "B",          "B");
            }
        }

        public sealed class LessThan_Comparer
        {
            [Fact]
            public void Throws_exception_when_reference_value_is_null()
            {
                Assert.Throws<ArgumentNullException>(() => Match.LessThan(null!, Comparer<string>.Default));
            }

            [Fact]
            public void Throws_exception_when_comparer_is_null()
            {
                Assert.Throws<ArgumentNullException>(() => Match.LessThan(string.Empty, null!));
            }

            [Theory]
            [MemberData(nameof(MatchesTheories))]
            public void Matches(bool expected, string value, string reference, IComparer<string> comparer)
            {
                var subject = Match.LessThan(reference, comparer);

                var actual = subject.Matches(value);

                Assert.Equal(expected, actual);
            }

            public static IEnumerable<object[]> MatchesTheories()
            {
                yield return DataTheory(true,  null,         "A", StringComparer.Ordinal);
                yield return DataTheory(true,  string.Empty, "A", StringComparer.Ordinal);
                yield return DataTheory(false, "A",          "A", StringComparer.Ordinal);
                yield return DataTheory(true,  "A",          "B", StringComparer.Ordinal);
                yield return DataTheory(false, "B",          "A", StringComparer.Ordinal);
                yield return DataTheory(false, "B",          "B", StringComparer.Ordinal);
                yield return DataTheory(false, "b",          "A", StringComparer.OrdinalIgnoreCase);
                yield return DataTheory(true,  "a",          "B", StringComparer.OrdinalIgnoreCase);
                yield return DataTheory(false, "b",          "B", StringComparer.OrdinalIgnoreCase);
            }
        }

        public sealed class LessThanOrEqualTo_Comparable
        {
            [Fact]
            public void Throws_exception_when_reference_value_is_null()
            {
                Assert.Throws<ArgumentNullException>(() => Match.LessThanOrEqualTo<string>(null));
            }

            [Theory]
            [MemberData(nameof(MatchesTheories))]
            public void Matches(bool expected, string value, string reference)
            {
                var subject = Match.LessThanOrEqualTo(reference);

                var actual = subject.Matches(value);

                Assert.Equal(expected, actual);
            }

            public static IEnumerable<object[]> MatchesTheories()
            {
                yield return DataTheory(true,  null,         "A");
                yield return DataTheory(true,  string.Empty, "A");
                yield return DataTheory(true,  "A",          "A");
                yield return DataTheory(true,  "A",          "B");
                yield return DataTheory(false, "B",          "A");
                yield return DataTheory(true,  "B",          "B");
            }
        }

        public sealed class LessThanOrEqualTo_Comparer
        {
            [Fact]
            public void Throws_exception_when_reference_value_is_null()
            {
                Assert.Throws<ArgumentNullException>(() => Match.LessThanOrEqualTo(null!, Comparer<string>.Default));
            }

            [Fact]
            public void Throws_exception_when_comparer_is_null()
            {
                Assert.Throws<ArgumentNullException>(() => Match.LessThanOrEqualTo(string.Empty, null!));
            }

            [Theory]
            [MemberData(nameof(MatchesTheories))]
            public void Matches(bool expected, string value, string reference, IComparer<string> comparer)
            {
                var subject = Match.LessThanOrEqualTo(reference, comparer);

                var actual = subject.Matches(value);

                Assert.Equal(expected, actual);
            }

            public static IEnumerable<object[]> MatchesTheories()
            {
                yield return DataTheory(true,  null,         "A", StringComparer.Ordinal);
                yield return DataTheory(true,  string.Empty, "A", StringComparer.Ordinal);
                yield return DataTheory(true,  "A",          "A", StringComparer.Ordinal);
                yield return DataTheory(true,  "A",          "B", StringComparer.Ordinal);
                yield return DataTheory(false, "B",          "A", StringComparer.Ordinal);
                yield return DataTheory(true,  "B",          "B", StringComparer.Ordinal);
                yield return DataTheory(false, "b",          "A", StringComparer.OrdinalIgnoreCase);
                yield return DataTheory(true,  "a",          "B", StringComparer.OrdinalIgnoreCase);
                yield return DataTheory(true,  "b",          "B", StringComparer.OrdinalIgnoreCase);
            }
        }

        public sealed class All
        {
            [Fact]
            public void Throws_exception_when_matches_are_null()
            {
                Assert.Throws<ArgumentNullException>(() => Match.All<int>((null!)));
            }

            [Fact]
            public void Throws_exception_when_matches_contain_null()
            {
                Assert.Throws<ArgumentException>(() => Match.All(Match.Always<int>(), null!, Match.Never<int>()));
            }

            [Theory]
            [MemberData(nameof(MatchesTheories))]
            public void Matches(bool expected, string value, Match<string>[] matches)
            {
                var subject = Match.All(matches);

                var actual = subject.Matches(value);

                Assert.Equal(expected, actual);
            }

            public static IEnumerable<object[]> MatchesTheories()
            {
                yield return DataTheory(false, string.Empty, MakeMatches(Match.Never<string>(),  Match.Never<string>(),                      Match.Never<string>()));
                yield return DataTheory(false, string.Empty, MakeMatches(Match.Always<string>(), Match.Never<string>(),                      Match.Never<string>()));
                yield return DataTheory(false, string.Empty, MakeMatches(Match.Never<string>(),  Match.Always<string>(),                     Match.Never<string>()));
                yield return DataTheory(false, string.Empty, MakeMatches(Match.Never<string>(),  Match.Never<string>(),                      Match.Always<string>()));
                yield return DataTheory(false, string.Empty, MakeMatches(Match.Always<string>(), Match.Always<string>(),                     Match.Never<string>()));
                yield return DataTheory(false, string.Empty, MakeMatches(Match.Always<string>(), Match.Never<string>(),                      Match.Always<string>()));
                yield return DataTheory(false, string.Empty, MakeMatches(Match.Never<string>(),  Match.Always<string>(),                     Match.Always<string>()));
                yield return DataTheory(true,  string.Empty, MakeMatches(Match.Always<string>(), Match.Always<string>(),                     Match.Always<string>()));
                yield return DataTheory(true,  "A",          MakeMatches(Match.Always<string>(), Match.EqualTo("A", StringComparer.Ordinal), Match.Always<string>()));
                yield return DataTheory(false, "A",          MakeMatches(Match.Always<string>(), Match.EqualTo("A", StringComparer.Ordinal), Match.Never<string>()));
                yield return DataTheory(false, "B",          MakeMatches(Match.Always<string>(), Match.EqualTo("A", StringComparer.Ordinal), Match.Always<string>()));
            }
        }

        public sealed class Any
        {
            [Fact]
            public void Throws_exception_when_matches_are_null()
            {
                Assert.Throws<ArgumentNullException>(() => Match.Any<int>((null!)));
            }

            [Fact]
            public void Throws_exception_when_matches_contain_null()
            {
                Assert.Throws<ArgumentException>(() => Match.Any(Match.Always<int>(), null!, Match.Never<int>()));
            }

            [Theory]
            [MemberData(nameof(MatchesTheories))]
            public void Matches(bool expected, string value, Match<string>[] matches)
            {
                var subject = Match.Any(matches);

                var actual = subject.Matches(value);

                Assert.Equal(expected, actual);
            }

            public static IEnumerable<object[]> MatchesTheories()
            {
                yield return DataTheory(false, string.Empty, MakeMatches(Match.Never<string>(),  Match.Never<string>(),                      Match.Never<string>()));
                yield return DataTheory(true,  string.Empty, MakeMatches(Match.Always<string>(), Match.Never<string>(),                      Match.Never<string>()));
                yield return DataTheory(true,  string.Empty, MakeMatches(Match.Never<string>(),  Match.Always<string>(),                     Match.Never<string>()));
                yield return DataTheory(true,  string.Empty, MakeMatches(Match.Never<string>(),  Match.Never<string>(),                      Match.Always<string>()));
                yield return DataTheory(true,  string.Empty, MakeMatches(Match.Always<string>(), Match.Always<string>(),                     Match.Never<string>()));
                yield return DataTheory(true,  string.Empty, MakeMatches(Match.Always<string>(), Match.Never<string>(),                      Match.Always<string>()));
                yield return DataTheory(true,  string.Empty, MakeMatches(Match.Never<string>(),  Match.Always<string>(),                     Match.Always<string>()));
                yield return DataTheory(true,  string.Empty, MakeMatches(Match.Always<string>(), Match.Always<string>(),                     Match.Always<string>()));
                yield return DataTheory(true,  "A",          MakeMatches(Match.Never<string>(),  Match.EqualTo("A", StringComparer.Ordinal), Match.Never<string>()));
                yield return DataTheory(false, "B",          MakeMatches(Match.Never<string>(),  Match.EqualTo("A", StringComparer.Ordinal), Match.Never<string>()));
            }
        }

        public sealed class None
        {
            [Fact]
            public void Throws_exception_when_matches_are_null()
            {
                Assert.Throws<ArgumentNullException>(() => Match.None<int>((null!)));
            }

            [Fact]
            public void Throws_exception_when_matches_contain_null()
            {
                Assert.Throws<ArgumentException>(() => Match.None(Match.Always<int>(), null!, Match.Never<int>()));
            }

            [Theory]
            [MemberData(nameof(MatchesTheories))]
            public void Matches(bool expected, string value, Match<string>[] matches)
            {
                var subject = Match.None(matches);

                var actual = subject.Matches(value);

                Assert.Equal(expected, actual);
            }

            public static IEnumerable<object[]> MatchesTheories()
            {
                yield return DataTheory(true,  string.Empty, MakeMatches(Match.Never<string>(),  Match.Never<string>(),                      Match.Never<string>()));
                yield return DataTheory(false, string.Empty, MakeMatches(Match.Always<string>(), Match.Never<string>(),                      Match.Never<string>()));
                yield return DataTheory(false, string.Empty, MakeMatches(Match.Never<string>(),  Match.Always<string>(),                     Match.Never<string>()));
                yield return DataTheory(false, string.Empty, MakeMatches(Match.Never<string>(),  Match.Never<string>(),                      Match.Always<string>()));
                yield return DataTheory(false, string.Empty, MakeMatches(Match.Always<string>(), Match.Always<string>(),                     Match.Never<string>()));
                yield return DataTheory(false, string.Empty, MakeMatches(Match.Always<string>(), Match.Never<string>(),                      Match.Always<string>()));
                yield return DataTheory(false, string.Empty, MakeMatches(Match.Never<string>(),  Match.Always<string>(),                     Match.Always<string>()));
                yield return DataTheory(false, string.Empty, MakeMatches(Match.Always<string>(), Match.Always<string>(),                     Match.Always<string>()));
                yield return DataTheory(false, "A",          MakeMatches(Match.Never<string>(),  Match.EqualTo("A", StringComparer.Ordinal), Match.Never<string>()));
                yield return DataTheory(true,  "B",          MakeMatches(Match.Never<string>(),  Match.EqualTo("A", StringComparer.Ordinal), Match.Never<string>()));
            }
        }

        public sealed class Except
        {
            [Fact]
            public void Throws_exception_when_matches_are_null()
            {
                Assert.Throws<ArgumentNullException>(() => Match.Except<int>((null!)));
            }

            [Fact]
            public void Throws_exception_when_matches_contain_null()
            {
                Assert.Throws<ArgumentException>(() => Match.Except(Match.Always<int>(), null!, Match.Never<int>()));
            }

            [Theory]
            [MemberData(nameof(MatchesTheories))]
            public void Matches(bool expected, string value, Match<string>[] matches)
            {
                var subject = Match.Except(matches);

                var actual = subject.Matches(value);

                Assert.Equal(expected, actual);
            }

            public static IEnumerable<object[]> MatchesTheories()
            {
                yield return DataTheory(true,  string.Empty, MakeMatches(Match.Never<string>(),  Match.Never<string>(),                      Match.Never<string>()));
                yield return DataTheory(false, string.Empty, MakeMatches(Match.Always<string>(), Match.Never<string>(),                      Match.Never<string>()));
                yield return DataTheory(false, string.Empty, MakeMatches(Match.Never<string>(),  Match.Always<string>(),                     Match.Never<string>()));
                yield return DataTheory(false, string.Empty, MakeMatches(Match.Never<string>(),  Match.Never<string>(),                      Match.Always<string>()));
                yield return DataTheory(false, string.Empty, MakeMatches(Match.Always<string>(), Match.Always<string>(),                     Match.Never<string>()));
                yield return DataTheory(false, string.Empty, MakeMatches(Match.Always<string>(), Match.Never<string>(),                      Match.Always<string>()));
                yield return DataTheory(false, string.Empty, MakeMatches(Match.Never<string>(),  Match.Always<string>(),                     Match.Always<string>()));
                yield return DataTheory(false, string.Empty, MakeMatches(Match.Always<string>(), Match.Always<string>(),                     Match.Always<string>()));
                yield return DataTheory(false, "A",          MakeMatches(Match.Never<string>(),  Match.EqualTo("A", StringComparer.Ordinal), Match.Never<string>()));
                yield return DataTheory(true,  "B",          MakeMatches(Match.Never<string>(),  Match.EqualTo("A", StringComparer.Ordinal), Match.Never<string>()));
            }
        }

        public sealed class Custom
        {
            [Fact]
            public void Throws_exception_when_custom_match_is_null()
            {
                Assert.Throws<ArgumentNullException>(() => Match.Custom<bool, CustomMatch<bool>>(null));
            }

            [Theory]
            [MemberData(nameof(MatchesTheories))]
            public void Matches(bool expected, bool value, CustomMatch<bool> customMatch)
            {
                var subject = Match.Custom<bool, CustomMatch<bool>>(customMatch);

                var actual = subject.Matches(value);

                Assert.Equal(expected, actual);
            }

            public static IEnumerable<object[]> MatchesTheories()
            {
                yield return DataTheory(true,  true,  CustomMatch<bool>.Create(true));
                yield return DataTheory(false, true,  CustomMatch<bool>.Create(false));
                yield return DataTheory(false, false, CustomMatch<bool>.Create(true));
                yield return DataTheory(true,  false, CustomMatch<bool>.Create(false));
            }
        }

        private static object[] DataTheory(params object[] values)
            => values;

        private static Match<T>[] MakeMatches<T>(params Match<T>[] matches)
            => matches;
    }

    public sealed class CustomMatch<T> : ICustomMatch<T>
        where T : IEquatable<T>
    {
        private readonly T expected;

        private CustomMatch(T expected)
        {
            this.expected = expected;
        }

        public static CustomMatch<T> Create(T expected)
            => new CustomMatch<T>(expected);

        public bool Matches(T value)
            => this.expected.Equals(value);
    }
}
