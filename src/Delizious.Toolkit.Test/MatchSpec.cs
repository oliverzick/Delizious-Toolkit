namespace Delizious.Toolkit.Test
{
    using Xunit;

    public sealed class MatchSpec
    {
        [Theory]
        [InlineData(true, false)]
        [InlineData(true, true)]
        public void Always(bool expected, bool value)
        {
            var subject = Match.Always<bool>();

            var actual = subject.Matches(value);

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(false, false)]
        [InlineData(false, true)]
        public void Never(bool expected, bool value)
        {
            var subject = Match.Never<bool>();

            var actual = subject.Matches(value);

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(true, null)]
        [InlineData(false, "")]
        [InlineData(false, "Sample string")]
        public void Null(bool expected, string value)
        {
            var subject = Match.Null<string>();

            var actual = subject.Matches(value);

            Assert.Equal(expected, actual);
        }
    }
}
