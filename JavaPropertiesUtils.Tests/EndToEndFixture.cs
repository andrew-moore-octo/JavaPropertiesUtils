using System;
using Assent;
using NUnit.Framework;

namespace JavaPropertiesUtils.Tests
{
    public class EndToEndFixture
    {
        [Test]
        public void Test()
        {
            var input = ResourceUtils.ReadEmbeddedResource("multiple-pairs-and-comments.properties");
            var parsed = Parser.Parse(input)
                .Set("key.2", "The new value for key 2")
                .Set("key=4", "The new value for key 4")
                .Set("Non existent key", "Should not be found.");

            var actual = parsed.ToString();
            this.Assent(actual);
        }
    }
}