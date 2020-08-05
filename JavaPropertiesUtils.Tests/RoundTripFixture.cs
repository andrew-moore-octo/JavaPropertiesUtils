using FluentAssertions;
using NUnit.Framework;

namespace JavaPropertiesUtils.Tests
{
    public class RoundTripFixture
    {
        [Test]
        [TestCase("blank-line.properties")]
        [TestCase("empty.properties")]
        [TestCase("exclamation-comment.properties")]
        [TestCase("hash-comment.properties")]
        [TestCase("key-with-colon-separator.properties")]
        [TestCase("key-with-equals-separator.properties")]
        [TestCase("key-with-logical-newline.properties")]
        [TestCase("key-with-neither-separator-nor-value.properties")]
        [TestCase("key-with-physical-newline-and-indentation.properties")]
        [TestCase("key-with-physical-newline.properties")]
        [TestCase("key-with-space-separator.properties")]
        [TestCase("multiple-pairs-and-comments.properties")]
        [TestCase("single-line.properties")]
        [TestCase("whitespace-before-comment.properties")]
        [TestCase("whitespace-before-key.properties")]
        public void FileCanRoundTrip(string resourceName)
        {
            var text = ResourceUtils.ReadEmbeddedResource(resourceName);
            var parsed = Parser.Parse(text);
            var roundTripped = parsed.ToString();
            text.Should().Be(roundTripped);
        }
    }
}