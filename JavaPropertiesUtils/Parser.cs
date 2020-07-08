using System;

namespace JavaPropertiesUtils
{
    public static class Parser
    {
        public static PropertiesDocument Parse(string input)
        {
            var tokenizer = new PropertiesFileTokenizer();
            var tokenizeResult = tokenizer.TryTokenize(input);
            if (!tokenizeResult.HasValue)
            {
                // TODO: error handling
                throw new NotImplementedException();
            }

            var parseResult = PropertiesFileParser.Parse(tokenizeResult.Value);
            if (!parseResult.HasValue)
            {
                // TODO: error handling
                throw new NotImplementedException();
            }

            return parseResult.Value;
        }
    }
}