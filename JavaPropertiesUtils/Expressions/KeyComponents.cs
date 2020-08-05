using System;
using Superpower.Model;

namespace JavaPropertiesUtils.Expressions
{
    public static class KeyComponents
    {
        public static StringValue GetStringValue(Token<TokenType> token)
        {
            var tokenString = token.ToStringValue();
            
            switch (token.Kind)
            {
                case TokenType.KeyChars:
                    return new StringValue(tokenString, tokenString);
                
                case TokenType.KeyPhysicalNewLine:
                    return tokenString.Contains("\r\n")
                        ? new StringValue(tokenString, "\r\n")
                        : new StringValue(tokenString, "\n");
                
                case TokenType.KeyEscapeSequence:
                    return new StringValue(tokenString, UnescapeString(tokenString));
                
                default:
                    throw new ArgumentOutOfRangeException(nameof(token), $"Can't handle token of type '{token.Kind}'.");
            }
        }

        private static string UnescapeString(string input)
        {
            switch (input)
            {
                case "\\r":
                    return "\r";
                case "\\n":
                    return "\n";
                case "\\t":
                    return "\t";
                case "\\ ":
                    return " ";
                case "\\:":
                    return ":";
                case "\\=":
                    return "=";
                case "\\\\":
                    return "\\";
                default:
                    // TODO: handle unicode escape.
                    throw new ArgumentOutOfRangeException(nameof(input), $"Unrecognised escaped string '{input}'");
            }
        }
    }
}