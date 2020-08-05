using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using JavaPropertiesUtils.Utils;

namespace JavaPropertiesUtils.Expressions
{
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
                        case KeyValuePair pair when pair.Key.Value.LogicalValue == key:
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

        protected bool Equals(PropertiesDocument other)
        {
            return Expressions.SequenceEqual(other.Expressions);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((PropertiesDocument) obj);
        }

        public override int GetHashCode()
        {
            return Expressions.AggregateHashCode();
        }
    }
}