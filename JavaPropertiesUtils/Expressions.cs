using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using Superpower.Model;

namespace JavaPropertiesUtils
{
    public interface IExpression
    {
    }

    public interface ITopLevelExpression : IExpression
    {
    }
    
    public class Key : IExpression
    {
        public Key(Token<TokenType>[] parts)
        {
            var tokenStringValue = string.Join(
                "", 
                parts.Select(p => p.ToStringValue())
            );
            
            EscapedName = tokenStringValue;
            Name = tokenStringValue
                .Replace("\\=", "=")
                .Replace("\\:", ":")
                .Replace("\\ ", " ")
                .Replace("\\\\", "\\");
        }
        
        public Key(string unescapedName)
        {
            Name = unescapedName;
            EscapedName = unescapedName
                .Replace(" ", "\\ ")
                .Replace("=", "\\=")
                .Replace(":", "\\:")
                .Replace("\\", "\\\\");
        }
        
        public string Name { get; }

        public string EscapedName { get; }

        public override string ToString()
        {
            return EscapedName;
        }
    }

    public class Separator : IExpression
    {
        public Separator(Token<TokenType> token)
        {
            Content = token.ToStringValue();
        }
        
        public string Content { get; }

        public override string ToString()
        {
            return Content;
        }
    }

    public class Value : IExpression
    {
        // TODO: arguments may change once we have multiline support.
        public Value(Token<TokenType> token)
        {
            Content = token.ToStringValue();
            // noship
            EscapedContent = token.ToStringValue();
        }

        public Value(string unescapedValue)
        {
            Content = unescapedValue;
            // noship
            EscapedContent = unescapedValue
                // TODO: handle unix newlines
                // TODO: nicer indentation?
                // noship
                .Replace("\r\n", "\\\r\n");
        }

        public string Content { get; }

        public string EscapedContent { get; }

        public override string ToString()
        {
            return EscapedContent;
        }
    }

    public class KeyValuePair : ITopLevelExpression
    {
        public KeyValuePair(Key key, Separator separator, Value value)
        {
            Key = key;
            Separator = separator;
            Value = value;
        }
        
        public Key Key { get; }
        
        public Separator Separator { get; }
        
        public Value Value { get; }

        public override string ToString()
        {
            return Key.ToString() + Separator + Value;
        }
    }
    
    public class Comment : ITopLevelExpression
    {
        public Comment(Token<TokenType> token)
        {
            Content = token.Span.ToStringValue();
        }
        
        public string Content { get; }

        public override string ToString()
        {
            return Content;
        }
    }

    public class NewLine : ITopLevelExpression
    {
        public NewLine(Token<TokenType> token)
        {
            Content = token.ToStringValue();
        }

        public string Content { get; }

        public override string ToString()
        {
            return Content;
        }
    }

    public class WhiteSpace : ITopLevelExpression
    {
        public WhiteSpace(Token<TokenType> token)
        {
            Content = token.ToStringValue();
        }

        public string Content { get; }

        public override string ToString()
        {
            return Content;
        }
    }

    public class PropertiesDocument
    {
        public PropertiesDocument(IEnumerable<ITopLevelExpression> expressions)
        {
            Expressions = expressions.ToImmutableArray();
        }

        public PropertiesDocument(params ITopLevelExpression[] expressions)
        {
            Expressions = expressions.ToImmutableArray();
        }
        
        public ImmutableArray<ITopLevelExpression> Expressions { get; }

        public PropertiesDocument Set(string key, string value)
        {
            var newExpressions = Expressions
                .Select(expr =>
                {
                    switch (expr)
                    {
                        case KeyValuePair pair when pair.Key.Name == key:
                            return new KeyValuePair(
                                pair.Key,
                                pair.Separator,
                                new Value(value)
                            );
                        
                        default:
                            return expr;
                    }
                });
            
            return new PropertiesDocument(newExpressions);
        }

        public override string ToString()
        {
            var result = new StringBuilder();
            
            foreach (var expr in Expressions)
            {
                result.Append(expr);
            }

            return result.ToString();
        }
    }
}