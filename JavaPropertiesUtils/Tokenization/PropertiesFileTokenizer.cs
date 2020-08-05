using System;
using System.Collections.Generic;
using Superpower;
using Superpower.Model;

namespace JavaPropertiesUtils.Tokenization
{
    public class PropertiesFileTokenizer : Tokenizer<TokenType>
    {
        // TODO: handle multiline values

        private static readonly TextParser<TokenType> StartOfFileParser = Comments.Parser
            .Or(Whitespace.Parser)
            .Or(NewLines.Parser)
            .Or(Keys.Parser);

        private static readonly TextParser<TokenType> AfterWhitespaceParser = Comments.Parser
            .Or(Keys.Parser);

        private static readonly TextParser<TokenType> AfterKeyComponentParser = Keys.Parser
            .Or(Separators.Parser)
            .Or(NewLines.Parser);

        private static readonly TextParser<TokenType> AfterSeparatorParser = Values.Parser
            .Or(NewLines.Parser);

        private static readonly TextParser<TokenType> AfterValueComponentParser = Comments.Parser
            .Or(Whitespace.Parser)
            .Or(NewLines.Parser)
            .Or(Keys.Parser);

        private static readonly TextParser<TokenType> AfterCommentParser = Whitespace.Parser;

        protected override IEnumerable<Result<TokenType>> Tokenize(TextSpan remainder, TokenizationState<TokenType> state)
        {
            // Parsing of the properties file syntax has to be stateful: any string can appear inside the value, so 
            // without knowing what the last token was, you don't know how to parse the remaining text. Hence why
            // we're overriding this overload that takes the state.
            
            while (!remainder.IsAtEnd)
            {
                var parserToTry = GetParserToTry(state);
                var parseResult = parserToTry(remainder);
                if (parseResult.HasValue)
                {
                    remainder = parseResult.Remainder;
                    yield return parseResult;
                }
                else
                {
                    yield return Result.Empty<TokenType>(remainder);
                }
            }
        }

        private static TextParser<TokenType> GetParserToTry(TokenizationState<TokenType> state)
        {
            switch (state?.Previous?.Kind)
            {
                case null:
                    return StartOfFileParser;
                
                case TokenType.KeyChars:
                case TokenType.KeyEscapeSequence:
                case TokenType.KeyPhysicalNewLine:
                    return AfterKeyComponentParser;
                
                case TokenType.Separator:
                    return AfterSeparatorParser;
                
                case TokenType.Value:
                    return AfterValueComponentParser;
                
                case TokenType.Comment:
                    return AfterCommentParser;
                
                case TokenType.Whitespace:
                    return AfterWhitespaceParser;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}