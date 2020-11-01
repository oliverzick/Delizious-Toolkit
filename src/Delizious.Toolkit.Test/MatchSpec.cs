namespace Delizious
{
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
    }
}
