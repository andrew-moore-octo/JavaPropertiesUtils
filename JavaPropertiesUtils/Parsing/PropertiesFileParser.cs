﻿using JavaPropertiesUtils.Expressions;
using JavaPropertiesUtils.Utils;
using Superpower;
using Superpower.Model;
using Superpower.Parsers;
using Comment = JavaPropertiesUtils.Expressions.Comment;

namespace JavaPropertiesUtils.Parsing
{
    public static class PropertiesFileParser
    {
        private static readonly TokenListParser<TokenType, Token<TokenType>> KeyComponent = Tokens.In(
            TokenType.KeyChars,
            TokenType.KeyEscapeSequence,
            TokenType.KeyPhysicalNewLine
        );
        
        public static readonly TokenListParser<TokenType, Key> KeyParser = 
            from tokens in KeyComponent.AtLeastOnce()
            select new Key(tokens);
        
        public static readonly TokenListParser<TokenType, Separator> SeparatorParser = 
            from token in Token.EqualTo(TokenType.Separator)
            select new Separator(token);
        
        public static readonly TokenListParser<TokenType, Value> ValueParser = 
            from token in Token.EqualTo(TokenType.Value)
            select new Value(token);
        
        public static readonly TokenListParser<TokenType, ITopLevelExpression> KeyValuePairParser =
            from key in KeyParser
            from separator in SeparatorParser.OptionalOrDefault()
            from value in ValueParser.OptionalOrDefault()
            select (ITopLevelExpression)new KeyValuePair(key, separator, value);

        public static readonly TokenListParser<TokenType, ITopLevelExpression> CommentParser = 
            from token in Token.EqualTo(TokenType.Comment)
            select (ITopLevelExpression)new Comment(token);
        
        public static readonly TokenListParser<TokenType, ITopLevelExpression> WhiteSpaceParser = 
            from token in Token.EqualTo(TokenType.Whitespace)
            select (ITopLevelExpression)new WhiteSpace(token);

        public static readonly TokenListParser<TokenType, ITopLevelExpression> TopLevelExpressions = KeyValuePairParser
            .Or(CommentParser)
            .Or(WhiteSpaceParser);

        public static readonly TokenListParser<TokenType, PropertiesDocument> DocumentParser = 
            from expressions in TopLevelExpressions.Many()
            select new PropertiesDocument(expressions);

        public static TokenListParserResult<TokenType, PropertiesDocument> Parse(TokenList<TokenType> tokens)
        {
            return DocumentParser(tokens);
        }
    }
}