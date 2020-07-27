using System;
using System.Collections.Generic;
using Superpower;
using Superpower.Model;
using JavaPropertiesUtils.Tokenization;

namespace JavaPropertiesUtils
{
    public class PropertiesFileTokenizer : Tokenizer<TokenType>
    {
        // TODO: handle multiline values
        // TODO: handle leading whitespace
        // TODO: handle whitespace (but not empty) lines

        private static readonly TextParser<TokenType> KeyOrWhitespaceOrNewLinesOrCommentParser = Comments.Parser
            .Or(Whitespace.Parser)
            .Or(NewLines.Parser)
            .Or(Keys.Parser);

        private static readonly TextParser<TokenType> KeyOrSeparatorOrNewLineParser = Keys.Parser
            .Or(Separators.Parser)
            .Or(NewLines.Parser);

        private static readonly TextParser<TokenType> ValueOrNewLineParser = Values.Parser
            .Or(NewLines.Parser);

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
                case TokenType.KeyChars:
                case TokenType.KeyEscapeSequence:
                case TokenType.KeyPhysicalNewLine:
                    return KeyOrSeparatorOrNewLineParser;
                
                case null:
                case TokenType.NewLine:
                case TokenType.Whitespace:
                case TokenType.Comment:
                case TokenType.Value:
                    return KeyOrWhitespaceOrNewLinesOrCommentParser;
                
                case TokenType.Separator:
                    return ValueOrNewLineParser;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}