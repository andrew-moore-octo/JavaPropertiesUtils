using System;
using System.Collections.Generic;
using Superpower;
using Superpower.Model;
using Superpower.Parsers;

namespace JavaPropertiesUtils
{
    public class PropertiesFileTokenizer : Tokenizer<TokenType>
    {
        // TODO: handle multiline values
        // TODO: handle leading whitespace
        // TODO: handle whitespace (but not empty) lines
        
        private static readonly TextParser<TextSpan> UntilNewLineParser = Span.WithoutAny(c => c == '\r' || c == '\n');
        private static readonly TextParser<char> ExclamationParser = Character.EqualTo('!');
        private static readonly TextParser<char> HashParser = Character.EqualTo('#');
        
        private static readonly TextParser<TokenType> CommentParser =
            from delimiter in ExclamationParser.Or(HashParser)
            from text in UntilNewLineParser
            select TokenType.Comment;

        private static readonly TextParser<TokenType> NewLineParser = Span
            .EqualTo("\n")
            .Or(Span.EqualTo("\r\n"))
            .Value(TokenType.NewLine);

        private static readonly TextParser<TextSpan> EscapedSpaceParser = Span.EqualTo("\\ ");
        private static readonly TextParser<TextSpan> EscapedColonParser = Span.EqualTo("\\:");
        private static readonly TextParser<TextSpan> EscapedEqualsParser = Span.EqualTo("\\=");
        private static readonly TextParser<TextSpan> EscapedBackSlashParser = Span.EqualTo("\\");

        private static readonly Func<char, bool> IsValidKeyCharacter = c =>
            c != ' ' && c != ':' && c != '=' && c != '\\' && !char.IsWhiteSpace(c);

        private static readonly TextParser<TextSpan> OtherValidKeyCharacters = Span.WithAll(IsValidKeyCharacter);

        private static readonly TextParser<TokenType> KeyParser = EscapedEqualsParser.Try()
            .Or(EscapedColonParser.Try())
            .Or(EscapedSpaceParser.Try())
            .Or(EscapedBackSlashParser.Try())
            .Or(OtherValidKeyCharacters)
            .AtLeastOnce()
            .Value(TokenType.Key);
            
        private static readonly TextParser<char> SpaceParser = Character.EqualTo(' ');
        private static readonly TextParser<char> ColonParser = Character.EqualTo(':');
        private static readonly TextParser<char> EqualsParser = Character.EqualTo('=');

        private static readonly TextParser<TokenType> SeparatorParser =
            from leadingWhitespace in SpaceParser.Many()
            from separator in ColonParser.Or(EqualsParser)
            from trailingWhitespace in SpaceParser.Many()
            select TokenType.Separator;

        // TODO: handle multiline values.
        private static readonly TextParser<TokenType> ValueParser = UntilNewLineParser.Value(TokenType.Value);
            
        protected override IEnumerable<Result<TokenType>> Tokenize(TextSpan remainder, TokenizationState<TokenType> state)
        {
            // Parsing of the properties file syntax has to be stateful: any string can appear inside the value, so 
            // without knowing what the last token was, you don't know how to parse the remaining text. Hence why
            // we're overriding this overload that takes the state.
            
            bool TryParser(TextParser<TokenType> parser, out Result<TokenType> result)
            {
                result = parser(remainder);
                if (result.HasValue)
                {
                    remainder = result.Remainder;
                }

                return result.HasValue;
            }
            
            while (!remainder.IsAtEnd)
            {
                var expectation = GetExpectation(state);

                switch (expectation)
                {
                    case TokenExpectation.KeyOrCommentOrBlank:
                        
                        if (TryParser(CommentParser, out var commentResult))
                        {
                            yield return commentResult;
                        }
                        else if (TryParser(NewLineParser, out var newLineResult))
                        {
                            yield return newLineResult;
                        }
                        else if (TryParser(KeyParser, out var keyResult))
                        {
                            yield return keyResult;
                        }
                        else
                        {
                            yield return Result.Empty<TokenType>(remainder);
                        }
                        break;
                        
                    case TokenExpectation.Separator:
                        if (TryParser(SeparatorParser, out var separatorResult))
                        {
                            yield return separatorResult;
                        }
                        else
                        {
                            yield return Result.Empty<TokenType>(remainder);
                        }
                        break;
                    
                    case TokenExpectation.Value:
                        if (TryParser(ValueParser, out var valueResult))
                        {
                            yield return valueResult;
                        }
                        else
                        {
                            yield return Result.Empty<TokenType>(remainder);
                        }
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private static TokenExpectation GetExpectation(TokenizationState<TokenType> state)
        {
            switch (state?.Previous?.Kind)
            {
                case TokenType.Key:
                    return TokenExpectation.Separator;
                    
                case TokenType.Separator:
                    return TokenExpectation.Value;
                
                default:
                    return TokenExpectation.KeyOrCommentOrBlank;
            }
        }
    }
}