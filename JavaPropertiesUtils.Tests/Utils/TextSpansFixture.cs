using FluentAssertions;
using JavaPropertiesUtils.Utils;
using NUnit.Framework;
using Superpower.Model;

namespace JavaPropertiesUtils.Tests.Utils
{
    [TestFixture]
    public class TextSpansFixture
    {
        public class Joins
        {
            [Test]
            public void ReturnsTheExpectedInputIfThereIsOnlyOneInput()
            {
                var source = "0123456789";
                var input = MakeSpan(source, 0, 5);
                var actual = TextSpans.Join(input);

                actual.Position.Absolute.Should().Be(0);
                actual.Length.Should().Be(5);
            }
            
            [Test]
            public void ReturnsTheExpectedSpanWithTwoInputs()
            {
                var source = "0123456789";
                var actual = TextSpans.Join(
                    MakeSpan(source, 1, 3),
                    MakeSpan(source, 4, 2)
                );
                actual.Position.Absolute.Should().Be(1);
                actual.Length.Should().Be(5);
            }

            [Test]
            public void ReturnsTheExpectedSpanWithMultipleInputsInNonSequentialOrder()
            {
                var source = "0123456789";
                var actual = TextSpans.Join(
                    MakeSpan(source, 2, 1),
                    MakeSpan(source, 1, 3),
                    MakeSpan(source, 3, 4)
                );
                actual.Position.Absolute.Should().Be(1);
                actual.Length.Should().Be(6);
            }

            private TextSpan MakeSpan(string source, int skip, int take)
            {
                return new TextSpan(source, Position.Zero, 0)
                    .Skip(skip)
                    .First(take);
            }
        }
    }
}