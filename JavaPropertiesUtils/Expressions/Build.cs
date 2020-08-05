using JavaPropertiesUtils.Tokenization;
using Superpower.Model;

namespace JavaPropertiesUtils.Expressions
{
    public static class Build
    {
        public static PropertiesDocument Doc(params ITopLevelExpression[] expressions)
        {
            return new PropertiesDocument(expressions);
        }

        public static Comment HashComment(string content)
        {
            return new Comment("#" + content);
        }

        public static Comment ExclamationComment(string content)
        {
            return new Comment("!" + content);
        }
        
        public static Comment BangComment(string content) => ExclamationComment(content);

        public static WhiteSpace Whitespace(string content)
        {
            return new WhiteSpace(content);
        }
        
        public static KeyValuePair Pair(Key key, Separator separator = null, Value value = null)
        {
            return new KeyValuePair(key, separator, value);
        }

        public static Key Key()
        {
            return new Key();
        }

        public static Token<TokenType> KeyChars(string serializableValue)
        {
            return new Token<TokenType>(TokenType.KeyChars, new TextSpan(serializableValue));
        }
    }
}