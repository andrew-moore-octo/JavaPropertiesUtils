using Superpower;
using Superpower.Parsers;

namespace JavaPropertiesUtils.Tokenization
{
    public static class Keys
    {
        private static readonly TextParser<TokenType> EscapeSequenceParser =
            from slash in Character.EqualTo('\\')
            from escaped in Character.In('r', 'n', 't', '\\', ':', '=', ' ')
            select TokenType.KeyEscapeSequence;

        private static readonly TextParser<TokenType> KeyCharParser = Span
            .WithAll(c => c != ' ' && c != '\\' && c != ':' && c != '=' && c != '\r' && c != '\r')
            .Value(TokenType.KeyChars);

        private static readonly TextParser<TokenType> PhysicalNewLineParser =
            from slash in Character.EqualTo('\\')
            from newline in Common.NewLineParser
            from indentation in Common.WhitespaceCharacterParser.Many()
            select TokenType.KeyPhysicalNewLine;

        public static readonly TextParser<TokenType> Parser = KeyCharParser
            .Or(EscapeSequenceParser.Try())
            .Or(PhysicalNewLineParser);
    }
}