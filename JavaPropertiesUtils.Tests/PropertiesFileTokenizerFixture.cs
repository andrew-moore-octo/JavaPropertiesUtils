using System;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using Superpower.Model;

namespace JavaPropertiesUtils.Tests
{
    public class PropertiesFileTokenizerFixture
    {
        private void RunTest(string inputResourceName, Action<TokenList<TokenType>> assertions)
        {
            
            var input = ResourceUtils.ReadEmbeddedResource(inputResourceName);
            var tokenizer = new PropertiesFileTokenizer();
            var tokens = tokenizer.Tokenize(input);

            assertions(tokens);
        }
        
        private void RunTest(string inputResourceName, params (TokenType TokenType, string TokenText)[] expected)
        {
            var expectations = expected
                .Select(ex => new Action<Token<TokenType>>(token =>
                {
                    token.Kind.Should().Be(ex.TokenType);
                    token.Span.ToStringValue().Should().Be(ex.TokenText);
                }));

            RunTest(inputResourceName, tokens => tokens.Should().SatisfyRespectively(expectations));
        }
        
        [Test]
        public void CanParseExclamationComment()
        {
            RunTest(
                "exclamation-comment.properties",
                (TokenType.Comment, "! This is a comment")
            );
        }
        
        [Test]
        public void CanParseHashComment()
        {
            RunTest(
                "hash-comment.properties",
                (TokenType.Comment, "# This is a comment")
            );
        }
        
        [Test]
        public void CanParseEmptyFile()
        {
            RunTest(
                "empty.properties",
                tokens => tokens.Should().BeEmpty()
            );
        }
        
        [Test]
        public void CanParseBlankLine()
        {
            RunTest(
                "blank-line.properties",
                (TokenType.NewLine, "\r\n")
            );
        }
        
        [Test]
        public void CanParseSingleLineFile()
        {
            RunTest(
                "single-line.properties",
                (TokenType.Key, "key"),
                (TokenType.Separator, "="),
                (TokenType.Value, "value")
            );
        }
        
        [Test]
        public void CanParseFileWithMultipleLinesAndComments()
        {
            RunTest(
                "multiple-pairs-and-comments.properties",
                (TokenType.Comment, "# Test file"),
                (TokenType.NewLine, "\r\n"),

                (TokenType.Key, "KEY1"),
                (TokenType.Separator, " = "),
                (TokenType.Value, "value1"),
                (TokenType.NewLine, "\r\n"),

                (TokenType.NewLine, "\r\n"),

                (TokenType.Comment, "! Another comment"),
                (TokenType.NewLine, "\r\n"),

                (TokenType.Key, "key.2"),
                (TokenType.Separator, ": "),
                (TokenType.Value, "another value "),
                (TokenType.NewLine, "\r\n"),

                (TokenType.Key, "key\\ 3"),
                (TokenType.Separator, " : "),
                (TokenType.Value, "third value"),
                (TokenType.NewLine, "\r\n"),

                (TokenType.Key, "key\\=4"),
                (TokenType.Separator, "=     "),
                (TokenType.Value, "fourth value"),
                (TokenType.NewLine, "\r\n"),
                
                (TokenType.Key, "key\\\\\\ \\:\\="),
                (TokenType.Separator, "="),
                (TokenType.Value, "fifth value"),
                (TokenType.NewLine, "\r\n")
            );
        }
    }
}