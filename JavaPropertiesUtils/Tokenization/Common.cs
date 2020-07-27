using Superpower;
using Superpower.Model;
using Superpower.Parsers;

namespace JavaPropertiesUtils.Tokenization
{
    public static class Common
    {
        public static readonly TextParser<TextSpan> UntilNewLineParser = Span
            .WithoutAny(c => c == '\r' || c == '\n');

        public static readonly TextParser<TextSpan> NewLineParser = Span
            .EqualTo("\n")
            .Or(Span.EqualTo("\r\n"));
    }
}