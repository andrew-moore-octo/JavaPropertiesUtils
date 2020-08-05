using System;
using System.Linq;
using FluentAssertions;
using JavaPropertiesUtils.Tokenization;
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
        public void CanParseWhitespaceBeforeAComment()
        {
            RunTest(
                "whitespace-before-comment.properties",
                (TokenType.Whitespace, "   "),
                (TokenType.Comment, "# This comment has leading whitespace")
            );
        }

        [Test]
        public void CanParseWhitespaceBeforeAKey()
        {
            RunTest(
                "whitespace-before-key.properties",
                (TokenType.Whitespace, "   "),
                (TokenType.KeyChars, "key"),
                (TokenType.Separator, "="),
                (TokenType.Value, "value")
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
                (TokenType.Whitespace, "\r\n")
            );
        }

        [Test]
        public void CanParseSingleLineFile()
        {
            RunTest(
                "single-line.properties",
                (TokenType.KeyChars, "key"),
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
                (TokenType.Whitespace, "\r\n"),

                (TokenType.KeyChars, "KEY1"),
                (TokenType.Separator, " = "),
                (TokenType.Value, "value1"),
                (TokenType.Whitespace, "\r\n\r\n"),

                (TokenType.Comment, "! Another comment"),
                (TokenType.Whitespace, "\r\n   "),

                (TokenType.KeyChars, "key.2"),
                (TokenType.Separator, ": "),
                (TokenType.Value, "another value "),
                (TokenType.Whitespace, "\r\n"),

                (TokenType.KeyChars, "key"),
                (TokenType.KeyEscapeSequence, "\\ "),
                (TokenType.KeyChars, "3"),
                (TokenType.Separator, " : "),
                (TokenType.Value, "third value"),
                (TokenType.Whitespace, "\r\n"),

                (TokenType.KeyChars, "key"),
                (TokenType.KeyEscapeSequence, "\\="),
                (TokenType.KeyChars, "4"),
                (TokenType.Separator, "=     "),
                (TokenType.Value, "fourth value"),
                (TokenType.Whitespace, "\r\n"),
                
                (TokenType.KeyChars, "key"),
                (TokenType.KeyEscapeSequence, "\\\\"),
                (TokenType.KeyEscapeSequence, "\\ "),
                (TokenType.KeyEscapeSequence, "\\:"),
                (TokenType.KeyEscapeSequence, "\\="),
                (TokenType.Separator, "="),
                (TokenType.Value, "fifth value"),
                (TokenType.Whitespace, "\r\n")
            );
        }

        [Test]
        public void CanParseAKeyWithNeitherSeparatorNorValue()
        {
            RunTest(
                "key-with-neither-separator-nor-value.properties",
                (TokenType.KeyChars, "key")
            );
        }

        [Test]
        public void CanParseAPairWithAColonSeparator()
        {
            RunTest(
                "key-with-colon-separator.properties",
                (TokenType.KeyChars, "key"),
                (TokenType.Separator, ":"),
                (TokenType.Value, "value")
            );
        }

        [Test]
        public void CanParseAPairWithASpaceSeparator()
        {
            RunTest(
                "key-with-space-separator.properties",
                (TokenType.KeyChars, "key"),
                (TokenType.Separator, " "),
                (TokenType.Value, "value")
            );
        }

        [Test]
        public void CanParseAPairWithAnEqualsSeparator()
        {
            RunTest(
                "key-with-equals-separator.properties",
                (TokenType.KeyChars, "key"),
                (TokenType.Separator, "="),
                (TokenType.Value, "value")
            );
        }

        [Test]
        public void CanParseAKeyWithALogicalNewline()
        {
            RunTest(
                "key-with-logical-newline.properties",
                (TokenType.KeyChars, "key"),
                (TokenType.KeyEscapeSequence, "\\n"),
                (TokenType.KeyChars, "1"),
                (TokenType.Separator, ":"),
                (TokenType.Value, "value")
            );
        }

        [Test]
        public void CanParseAKeyWithAPhysicalNewline()
        {
            RunTest(
                "key-with-physical-newline.properties",
                (TokenType.KeyChars, "ke"),
                (TokenType.KeyPhysicalNewLine, "\\\r\n"),
                (TokenType.KeyChars, "y"),
                (TokenType.Separator, ":"),
                (TokenType.Value, "value")
            );
        }

        [Test]
        public void CanParseAKeyWithAPhysicalNewlineAndIndentation()
        {
            RunTest(
                "key-with-physical-newline-and-indentation.properties",
                (TokenType.KeyChars, "ke"),
                (TokenType.KeyPhysicalNewLine, "\\\r\n   "),
                (TokenType.KeyChars, "y"),
                (TokenType.Separator, ":"),
                (TokenType.Value, "value")
            );
        }
    }
}